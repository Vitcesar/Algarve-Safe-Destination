using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.Storage.FileProperties;
using Windows.Security.Cryptography.Core;
using Windows.Security.Cryptography;
using Windows.ApplicationModel;

namespace ADS.Classes
{
    public class Update
    {
        List<Ficheiro> fl;

        public Update()
        {
            getnewfiles();
            //syncDB();
        }

        async private void syncDB()
        {
            DatabaseHelperClass dbhandle = new DatabaseHelperClass();
            var client = new HttpClient(); // Add: using System.Net.Http;
            var response = await client.GetStringAsync(new Uri("http://10.4.0.98/conng2.php"));
            JObject o = JObject.Parse(response);

            if (o.Count > 0)
            {
                foreach (KeyValuePair<string, JToken> item in o)
                {
                    string key = item.Key;
                    JToken jToken = item.Value;
                    string value = jToken.ToString();
                    if (key.Contains("Topic"))
                    {
                        Topico top = new Topico();
                        top.ID_Tema = jToken.ElementAt(0).ToString();
                        top.Titulo = jToken.ElementAt(1).ToString();
                        top.Descricao = jToken.ElementAt(2).ToString();
                        top.Visibilidade = jToken.ElementAt(3).ToString();
                        top.Icone = jToken.ElementAt(4).ToString();
                        dbhandle.Insert(top);
                    }

                    else if (key.Contains("SubTopic"))
                    {
                        Subtopico subtop = new Subtopico();
                        subtop.ID_Tema = jToken.ElementAt(0).ToString();
                        subtop.ID_Topico = jToken.ElementAt(1).ToString();
                        subtop.Titulo = jToken.ElementAt(2).ToString();
                        subtop.Descricao = jToken.ElementAt(3).ToString();
                        subtop.Visibilidade = jToken.ElementAt(4).ToString();
                        subtop.Icone = jToken.ElementAt(5).ToString();
                        dbhandle.Insert(subtop);
                    }

                    else if (key.Contains("ThemeContent"))
                    {
                        Conteudo_Tema contem = new Conteudo_Tema();
                        contem.ID_Tema = jToken.ElementAt(0).ToString();
                        contem.Codigo_Idioma = jToken.ElementAt(1).ToString();
                        contem.Titulo = jToken.ElementAt(2).ToString();
                        contem.Visibilidade = jToken.ElementAt(3).ToString();
                        contem.Documento = jToken.ElementAt(4).ToString();
                        dbhandle.Insert(contem);
                    }

                    else if (key.Contains("TopicContent"))
                    {
                        Conteudo_Topico contop = new Conteudo_Topico();
                        contop.ID_Topico = jToken.ElementAt(0).ToString();
                        contop.ID_Tema = jToken.ElementAt(1).ToString();
                        contop.Codigo_Idioma = jToken.ElementAt(2).ToString();
                        contop.Titulo = jToken.ElementAt(3).ToString();
                        contop.Visibilidade = jToken.ElementAt(4).ToString();
                        contop.Documento = jToken.ElementAt(5).ToString();
                        dbhandle.Insert(contop);
                    }

                    else if (key.Contains("SubTopicContent"))
                    {
                        Conteudo_Subtopico consubtop = new Conteudo_Subtopico();
                        consubtop.ID_Subtopico = jToken.ElementAt(0).ToString();
                        consubtop.ID_Topico = jToken.ElementAt(1).ToString();
                        consubtop.ID_Tema = jToken.ElementAt(2).ToString();
                        consubtop.Codigo_Idioma = jToken.ElementAt(3).ToString();
                        consubtop.Titulo = jToken.ElementAt(4).ToString();
                        consubtop.Visibilidade = jToken.ElementAt(5).ToString();
                        consubtop.Documento = jToken.ElementAt(6).ToString();
                        dbhandle.Insert(consubtop);
                    }

                    else if (key.Contains("Language"))
                    {
                        Idioma idi = new Idioma();
                        idi.Codigo_Idioma = jToken.ElementAt(0).ToString();
                        idi.Nome = jToken.ElementAt(1).ToString();
                        dbhandle.Insert(idi);
                    }

                    else if (key.Contains("Phrase"))
                    {
                        /*Palavra p = new Palavra();
                        p.Codigo_Frase = jToken.ElementAt(0).ToString();
                        p.Codigo_Idioma = jToken.ElementAt(1).ToString();
                        p.Codigo_Ascendente = jToken.ElementAt(2).ToString();
                        p.Frase = jToken.ElementAt(3).ToString();*/
                        //dbhandle.Insert(p);
                    }

                    else if (key.Contains("EntityType"))
                    {
                        Tipo_Entidade tipoent = new Tipo_Entidade();
                        tipoent.Nome = jToken.ElementAt(0).ToString();
                        dbhandle.Insert(tipoent);
                    }

                    else if (key.Contains("EntityTypeLanguage"))
                    {
                        Tipo_Entidade_Idioma tipoentidi = new Tipo_Entidade_Idioma();
                        tipoentidi.Codigo_Idioma = jToken.ElementAt(0).ToString();
                        tipoentidi.Nome_Tipo_Entidade = jToken.ElementAt(1).ToString();
                        tipoentidi.Nome_Idioma = jToken.ElementAt(2).ToString();
                        dbhandle.Insert(tipoentidi);
                }

                    else if (key.Contains("EntityLanguage"))
                    {
                        Entidade_Idioma entidi = new Entidade_Idioma();
                        entidi.Codigo_Idioma = jToken.ElementAt(0).ToString();
                        entidi.Nome_Entidade = jToken.ElementAt(1).ToString();
                        entidi.Codigo_Postal_Entidade = jToken.ElementAt(2).ToString();
                        entidi.Nome_Idioma = jToken.ElementAt(3).ToString();
                        dbhandle.Insert(entidi);
            }

                    else if (key.Contains("Entity"))
                    {
                        /*Entidade ent = new Entidade();
                        ent.Nome = jToken.ElementAt(0).ToString();
                        ent.Codigo_Postal = jToken.ElementAt(1).ToString();
                        ent.Codigo_Pais = jToken.ElementAt(2).ToString();
                        ent.Nome_Tipo_Entidade = jToken.ElementAt(3).ToString();
                        ent.????? = jToken.ElementAt(4).ToString();
                        ent.????? = jToken.ElementAt(5).ToString();                     //N_Vertice?
                        ent.Telefone = Convert.ToInt32(jToken.ElementAt(6));
                        ent.Morada = jToken.ElementAt(7).ToString();
                        ent.Anexo = jToken.ElementAt(8).ToString();
                        dbhandle.Insert(ent);*/
                    }

                    else if (key.Contains("Vertex"))
                    {
                        Vertice vert = new Vertice();
                        vert.Latitude = Convert.ToInt32(jToken.ElementAt(0));
                        vert.Longitude = Convert.ToInt32(jToken.ElementAt(1));
                        //N_Vertice?
                        dbhandle.Insert(vert);
                    }

                    else if (key.Contains("Country"))
                    {
                        Pais pais = new Pais();
                        pais.Codigo_Pais = jToken.ElementAt(0).ToString();
                        pais.Nome = jToken.ElementAt(1).ToString();
                        dbhandle.Insert(pais);
                }
                    //Debug.WriteLine("key: " + key + " value: " + value);
                
                }
                //Debug.WriteLine(o);
                //string json = await response.Content.ReadAsStringAsync();
            }
        }

        async private void getnewfiles()
        {
            var client = new HttpClient(); // Add: using System.Net.Http;
            var response = await client.GetAsync(new Uri("http://10.4.0.98/getnewfiles.php"));
            string json = await response.Content.ReadAsStringAsync();


            List<Ficheiro> fl = new List<Ficheiro>();

            JObject obj = JObject.Parse(json);
            if (obj.Count > 0)
            {
                foreach (KeyValuePair<string, JToken> item in obj)
                {
                    string key = item.Key;
                    JToken jToken = item.Value;
                    string value = jToken.ToString();
                    fl.Add(new Ficheiro(key, value));
                }
            }

            this.fl = fl;

            //foreach (var f in fl)
            //{
            //    Debug.WriteLine("Nome = " + f.nome);
            //    Debug.WriteLine("MD5 = " + f.checksum);
            //}

            download();
        }


        private async void download()
        {
            //var client = new HttpClient(); // Add: using System.Net.Http;

            foreach (var f in fl)
            {
                StorageFolder appfolder = await ApplicationData.Current.LocalFolder.CreateFolderAsync("Assets", CreationCollisionOption.OpenIfExists);
                int empty = 0;
                
                foreach (StorageFile file2 in await appfolder.GetItemsAsync())
                {
                    //Debug.WriteLine(f.nome + " " + f.checksum);
                    //Debug.WriteLine(file2.DisplayName + file2.FileType + " " + await ComputeMD5(file2));
                    if(f.nome == file2.DisplayName+file2.FileType)
                    {
                        if (f.checksum != await ComputeMD5(file2))
                        {
                            StorageFile file = await appfolder.CreateFileAsync(f.nome, CreationCollisionOption.ReplaceExisting);

                            string url = "http://10.4.0.98/downloads/" + f.nome;
                            HttpClient client = new HttpClient();

                            byte[] responseBytes = await client.GetByteArrayAsync(url);

                            using (var stream = await file.OpenAsync(FileAccessMode.ReadWrite))
                            {
                                using (var outputStream = stream.GetOutputStreamAt(0))
                                {
                                    DataWriter writer = new DataWriter(outputStream);
                                    writer.WriteBytes(responseBytes);
                                    await writer.StoreAsync();
                                    await outputStream.FlushAsync();
                                }
                            }

                            var bs = await file.GetBasicPropertiesAsync();
                            Debug.WriteLine("Path of the downloaded file: " + file.Path.ToString());
                            Debug.WriteLine("Type of the downloaded file: " + file.DisplayType + "    Size of the file: " + bs.Size);
                            //Debug.WriteLine("Hash: " + await ComputeMD5(file));
                            Debug.WriteLine("");
                        }
                    }

                    else { empty++; }
                }

                var qq = await appfolder.GetFilesAsync();
                if (empty == qq.Count)
                {
                    StorageFile file3 = await appfolder.CreateFileAsync(f.nome, CreationCollisionOption.ReplaceExisting);

                    string url2 = "http://10.4.0.98/downloads/" + f.nome;
                    HttpClient client2 = new HttpClient();

                    byte[] responseBytes2 = await client2.GetByteArrayAsync(url2);

                    using (var stream = await file3.OpenAsync(FileAccessMode.ReadWrite))
                    {
                        using (var outputStream = stream.GetOutputStreamAt(0))
                        {
                            DataWriter writer = new DataWriter(outputStream);
                            writer.WriteBytes(responseBytes2);
                            await writer.StoreAsync();
                            await outputStream.FlushAsync();
                        }
                    }

                    var bls = await file3.GetBasicPropertiesAsync();
                    Debug.WriteLine("Path of the downloaded file: " + file3.Path.ToString());
                    Debug.WriteLine("Type of the downloaded file: " + file3.DisplayType + "    Size of the file: " + bls.Size);
                    //Debug.WriteLine("Hash: " + await ComputeMD5(file));
                    Debug.WriteLine("");
                }
            }

            Pages.Definicoes.instance.block_update();
        }

        private const int CHUNK_SIZE = 1024;
        private async Task<string> ComputeMD5(IStorageFile file)
        {

            string res = string.Empty;
            using (var streamReader = await file.OpenAsync(FileAccessMode.Read))
            {
                var alg = HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Md5);
                var algHash = alg.CreateHash();

                using (BinaryReader reader = new BinaryReader(streamReader.AsStream()))
                {
                    byte[] chunk;

                    chunk = reader.ReadBytes(CHUNK_SIZE);
                    while (chunk.Length > 0)
                    {
                        algHash.Append(CryptographicBuffer.CreateFromByteArray(chunk));
                        chunk = reader.ReadBytes(CHUNK_SIZE);
                    }
                }

                res = CryptographicBuffer.EncodeToHexString(algHash.GetValueAndReset());
                return res;
            }
        }

        /*public async Task SaveToLocalFolderAsync(Stream file, string fileName)
        {
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            StorageFile storageFile = await localFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
            using (Stream outputStream = await storageFile.OpenStreamForWriteAsync())
            {
                await file.CopyToAsync(outputStream);
                Debug.WriteLine(outputStream.ToString());
            }
        }*/

    }


    public class Ficheiro
    {
        public string nome { get; set; }
        public string checksum { get; set; }

        public Ficheiro(string nome, string checksum)
        {
            this.nome = nome;
            this.checksum = checksum;
        }
    }
}
