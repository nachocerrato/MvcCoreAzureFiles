using Azure.Storage.Files.Shares;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MvcCoreAzureFiles.Services
{
    public class ServiceStorageFile
    {
        //Todo funciona por recursos compartidos
        //Necesitamos el nombre del recurso compartido
        private ShareDirectoryClient root;

        //Para acceder a los ficheros necesitamos unas claves del portal de azure
        public ServiceStorageFile(string keys)
        {
            //Creamos cliente apuntando al shared que hemos creado antes
            ShareClient client =
                new ShareClient(keys, "ejemplofiles");
            //Indicamos que recupere la ruta a dicho directorio
            this.root = client.GetRootDirectoryClient();
        }

        //Mostrar todos los ficheros
        public async Task<List<string>> GetFilesAsync()
        {
            List<string> files = new List<string>();
            await foreach (var file in root.GetFilesAndDirectoriesAsync())
            {
                files.Add(file.Name);
            }
            return files;
        }

        //Método para leer el contenido de un fichero
        public async Task<string> ReadFileAsync(string filename)
        {
            ShareFileClient file = this.root.GetFileClient(filename);
            var data = await file.DownloadAsync();
            Stream stream = data.Value.Content;
            StreamReader reader = new StreamReader(stream);
            return await reader.ReadToEndAsync();

        }

        //Método para subir el archivo a azure
        
        public async Task UploadFileAsync(string filename, Stream stream)
        {
            ShareFileClient file = this.root.GetFileClient(filename);
            await file.CreateAsync(stream.Length);
            await file.UploadAsync(stream);
        }

        //Método para eliminar un fichero
        public async Task DeleteFileAsync(string filename)
        {
            ShareFileClient file = this.root.GetFileClient(filename);
            await file.DeleteAsync();
        }
    }
}
