using CLIReader;
using EFCore.Helper;
using Serilog;

namespace CLIWizardHelper;

public abstract class InsertWizard<TUnitOfWork, TEntity> 
    : IInsertWizard<TEntity> 
        where TEntity : new()
    	where TUnitOfWork : IUnitOfWork
{
    protected readonly TUnitOfWork UnitOfWork;
    protected readonly IReader<string> RequiredTextReader;
    private readonly ILogger log;

    public InsertWizard(
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

    public virtual void Insert()
    {
        try
        {
            var model = GetEntity();
            InsertEntity(model);
            UnitOfWork.Save();
        }
        catch (Exception ex)
        {
            log.Error(ex, "Insert Error");
        }
    }

    protected abstract TEntity GetEntity();
    
    protected abstract void InsertEntity(TEntity entity);
}