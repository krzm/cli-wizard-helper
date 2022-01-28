using CLIHelper;
using CLIReader;
using EFCoreHelper;
using ModelHelper;

namespace CLIWizardHelper;

public abstract class UpdateWizard<TUnitOfWork, TEntity> 
	: IUpdateWizard<TEntity>
        where TUnitOfWork : IUnitOfWork
{
    protected readonly TUnitOfWork UnitOfWork;
    protected readonly IReader<string> RequiredTextReader;
    private readonly IOutput output;

    public UpdateWizard(
        TUnitOfWork unitOfWork
        , IReader<string> requiredTextReader
        , IOutput output)
    {
        UnitOfWork = unitOfWork;
        RequiredTextReader = requiredTextReader;
        this.output = output;

        ArgumentNullException.ThrowIfNull(UnitOfWork);
        ArgumentNullException.ThrowIfNull(RequiredTextReader);
        ArgumentNullException.ThrowIfNull(this.output);
    }

    public virtual void Update()
    {
        try
        {
            var id = int.Parse(
                RequiredTextReader.Read(
                    new ReadConfig(
                        6
                        , $"Select {typeof(TEntity).Name} Id")));
            var model = GetById(id);
            if (model == null)
                throw new Exception($"No data in database for Id {id}");
            var nr = int.Parse(
                RequiredTextReader.Read(
                    new ReadConfig(
                        1
                        , GetPropsSelectMenu())));
            UpdateEntity(nr, model);
            UnitOfWork.Save();
        }
        catch (ArgumentException ex)
        {
            output.WriteLine(ex.Message);
        }
        catch (Exception ex)
        {
            output.WriteLine(ex.Message);
        }
    }

    protected virtual string GetPropsSelectMenu()
    {
        return $"Select property number. 1-{nameof(IModelA.Name)}, 2-{nameof(IModelA.Description)}";
    }

    protected abstract TEntity GetById(int id);

    protected abstract void UpdateEntity(int nr, TEntity model);
}