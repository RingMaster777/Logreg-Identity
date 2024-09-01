namespace LogReg_Identity.Repository.IRepository
{
    public interface IUnitOfWork
    {
        INoteRepository Note { get; }
        IMenuRepository Menu { get; }

        IMenuPermissionRepository MenuPermission { get; }
        void Save();
    }
}
