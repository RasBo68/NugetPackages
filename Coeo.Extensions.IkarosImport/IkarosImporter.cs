namespace Coeo.Extensions.IkarosImport
{
    public class IkarosImporter
    {
        public void Import<T>(T entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            // Implementation for importing to Ikaros
        }
    }
}
