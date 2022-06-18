using CLIReader;
using EFCore.Helper;
using ModelHelper;
using Serilog;

namespace CLIWizardHelper;

public abstract class UpdateWizard<TUnitOfWork, TEntity> 
	: IUpdateWizard<TEntity>
        where TUnitOfWork : IUnitOfWork
{
    protected readonly TUnitOfWork UnitOfWork;
    protected readonly IReader<string> RequiredTextReader;
    private readonly ILogger log;

    public UpdateWizard(
        TUnitOfWork unitOfWork
        , IReader<string> requiredTextReader
        , ILogger log)
    {
        UnitOfWork = unitOfWork;
        RequiredTextReader = requiredTextReader;
        this.log = log;

        ArgumentNullException.ThrowIfNull(UnitOfWork);
        ArgumentNullException.ThrowIfNull(RequiredTextReader);
        ArgumentNullException.ThrowIfNull(this.log);
    }

    public virtual void Update()
    {
        try
        {
            var idInput = RequiredTextReader.Read(
                new ReadConfig(
                    6
                    , $"Select {typeof(TEntity).Name} Id"));
            ArgumentNullException.ThrowIfNull(idInput);
            var id = int.Parse(idInput);
            var model = GetById(id);
            ArgumentNullException.ThrowIfNull(model);
            var nrInput = RequiredTextReader.Read(
                new ReadConfig(
                    1
                    , GetPropsSelectMenu()));
            ArgumentNullException.ThrowIfNull(nrInput);
            var nr = int.Parse(nrInput);
            UpdateEntity(nr, model);
            UnitOfWork.Save();
        }
        catch (Exception ex)
        {
            log.Error(ex, "Update Error");
        }
    }

    protected virtual string GetPropsSelectMenu()
    {
        return $"Select property number. 1-{nameof(IModelA.Name)}, 2-{nameof(IModelA.Description)}";
    }

    protected abstract TEntity GetById(int id);

    protected abstract void UpdateEntity(int nr, TEntity model);
}