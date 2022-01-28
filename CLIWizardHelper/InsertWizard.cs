using CLIHelper;
using CLIReader;
using EFCoreHelper;

namespace CLIWizardHelper;

public abstract class InsertWizard<TUnitOfWork, TEntity> 
    : IInsertWizard<TEntity> 
        where TEntity : new()
    	where TUnitOfWork : IUnitOfWork
{
    protected readonly TUnitOfWork UnitOfWork;
    protected readonly IReader<string> RequiredTextReader;
    private readonly IOutput output;

    public InsertWizard(
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

    public virtual void Insert()
    {
        try
        {
            var model = GetEntity();
            InsertEntity(model);
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

    protected abstract TEntity GetEntity();
    
    protected abstract void InsertEntity(TEntity entity);
}