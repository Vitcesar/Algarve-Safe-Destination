using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using ADS.Classes;
using System.Globalization;
using System.Diagnostics;
using Windows.Storage;
using System.Text.RegularExpressions;

namespace ADS.Classes
{
    public class DatabaseHelperClass
    {
        SQLiteConnection dbConn;
        ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

        //Create Tabble
        public async Task<bool> onCreate(string DB_PATH)
        {
            try
            {
                if (!CheckFileExists(DB_PATH).Result)
                {
                    using (dbConn = new SQLiteConnection(DB_PATH))
                    {
                        dbConn.CreateTable<Tema>();
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        private async Task<bool> CheckFileExists(string fileName)
        {
            try
            {
                var store = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFileAsync(fileName);
                return true;
            }
            catch
            {
                return false;
            }
        }

        //Start search on database
        public ObservableCollection<Search_Result> Search(string s)
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                ObservableCollection<Search_Result> ResultList = new ObservableCollection<Search_Result>();

                List<Conteudo_Tema> myTemaSearch = dbConn.Query<Conteudo_Tema>("select * from Conteudo_Tema where Titulo LIKE '%" + s + "%' COLLATE NOCASE AND Codigo_Idioma = '" + localSettings.Values["Idioma"] + "';").ToList<Conteudo_Tema>();
                ObservableCollection<Conteudo_Tema> TemasList = new ObservableCollection<Conteudo_Tema>(myTemaSearch);
                foreach (var ts in TemasList)
                {
                    Search_Result result = new Search_Result(ts, null, null, null, "tema", ts.Titulo);
                    ResultList.Add(result);
                }

                List<Conteudo_Topico> myTopicoSearch = dbConn.Query<Conteudo_Topico>("select * from Conteudo_Topico where Titulo LIKE '%" + s + "%' COLLATE NOCASE AND Codigo_Idioma = '" + localSettings.Values["Idioma"] + "';").ToList<Conteudo_Topico>();
                ObservableCollection<Conteudo_Topico> TopicosList = new ObservableCollection<Conteudo_Topico>(myTopicoSearch);
                foreach (var ts in TopicosList)
                {
                    Search_Result result = new Search_Result(null, ts, null, null, "topico", ts.Titulo);
                    ResultList.Add(result);
                }

                List<Conteudo_Subtopico> mySubtopicoSearch = dbConn.Query<Conteudo_Subtopico>("select * from Conteudo_Subtopico where Titulo LIKE '%" + s + "%' COLLATE NOCASE AND Codigo_Idioma = '" + localSettings.Values["Idioma"] + "';").ToList<Conteudo_Subtopico>();
                ObservableCollection<Conteudo_Subtopico> SubtopicosList = new ObservableCollection<Conteudo_Subtopico>(mySubtopicoSearch);
                foreach (var ts in SubtopicosList)
                {
                    Search_Result result = new Search_Result(null, null, ts, null, "subtopico", ts.Titulo);
                    ResultList.Add(result);
                }

                
                /*List<Entidade_Idioma> myEntSearch = dbConn.Query<Entidade_Idioma>("select * from Entidade_Idioma where Nome_Idioma LIKE '%" + s + "%' COLLATE NOCASE AND Codigo_Idioma = '" + localSettings.Values["Idioma"] + "';").ToList<Entidade_Idioma>();
                ObservableCollection<Entidade_Idioma> EntList = new ObservableCollection<Entidade_Idioma>(myEntSearch);
                foreach (var ts in EntList)
                {
                    
                    Search_Result result = new Search_Result(null, null, null, ts, "entidade", ts.Nome_Idioma);
                    ResultList.Add(result);
                }*/

                ObservableCollection<Classes.Entidade> EntList = ReadEntidade();
                foreach (var ent in EntList)
                {
                    if (Regex.IsMatch(ent.Nome,s, RegexOptions.IgnoreCase))
                    {
                        Search_Result result = new Search_Result(null, null, null, ent, "entidade", ent.Nome);
                        ResultList.Add(result);
                    }
                }
              
                return ResultList;
            }
        }

        // Retrieve the specific theme from the database.
        public Tema ReadTemas(string temasid)
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                var existingconact = dbConn.Query<Tema>("select * from Tema where Titulo = '" + temasid + "'").FirstOrDefault();
                return existingconact;
            }
        }


        public Conteudo_Tema ReadTemaTitle(string temasid)
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                var existingconact = dbConn.Query<Conteudo_Tema>("select * from Conteudo_Tema where ID_Tema = '" + temasid + "' AND Codigo_Idioma = '" + localSettings.Values["Idioma"] + "';").FirstOrDefault();
                return existingconact;
            }
        }

        public Conteudo_Topico ReadTopicTitle(string topicid)
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                var existingconact = dbConn.Query<Conteudo_Topico>("select * from Conteudo_Topico where ID_Topico = '" + topicid + "' AND Codigo_Idioma = '" + localSettings.Values["Idioma"] + "';").FirstOrDefault();
                return existingconact;
            }
        }

        public Conteudo_Subtopico ReadSubtopicTitle(string subtopicid)
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                var existingconact = dbConn.Query<Conteudo_Subtopico>("select * from Conteudo_Subtopico where ID_Subtopico = '" + subtopicid + "' AND Codigo_Idioma = '" + localSettings.Values["Idioma"] + "';").FirstOrDefault();
                return existingconact;
            }
        }

        // Retrieve the all Conteudo list from the database.
        public ObservableCollection<Conteudo_Tema> ReadTemas()
        {

            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                List<Tema> myCollection = dbConn.Table<Tema>().ToList<Tema>();
                List<Conteudo_Tema> myContent_List = new List<Conteudo_Tema>();
                foreach (var ts in myCollection)
                {
                    Conteudo_Tema myContent = dbConn.Query<Conteudo_Tema>("select * from Conteudo_Tema where ID_Tema ='" + ts.Titulo + "' AND Codigo_Idioma = '" + localSettings.Values["Idioma"] + "';").FirstOrDefault();
                    //Conteudo myContent = dbConn.Query<Conteudo>("select * from Conteudo where id ='" + myContent_lang.id_conteudo + "';").FirstOrDefault();
                    myContent.parent_tema = ts;
                    myContent_List.Add(myContent);
                }
                ObservableCollection<Conteudo_Tema> TemasList = new ObservableCollection<Conteudo_Tema>(myContent_List);
                return TemasList;
            }
        }

        public ObservableCollection<Conteudo_Topico> ReadTopicos(string idtema)
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                List<Topico> myCollection = dbConn.Query<Topico>("select * from Topico where ID_Tema = '" + idtema + "'").ToList<Topico>();

                List<Conteudo_Topico> myContent_List = new List<Conteudo_Topico>();
                foreach (var ts in myCollection)
                {
                    Conteudo_Topico myContent = dbConn.Query<Conteudo_Topico>("select * from Conteudo_Topico WHERE ID_Topico = '" + ts.Titulo + "' AND Codigo_Idioma = '" + localSettings.Values["Idioma"] + "';").FirstOrDefault();
                    //Conteudo myContent = dbConn.Query<Conteudo>("select * from Conteudo where id ='" + myContent_lang.id_conteudo + "';").FirstOrDefault();
                    myContent.parent_topico = ts;
                    myContent_List.Add(myContent);
                }
                ObservableCollection<Conteudo_Topico> TemasList = new ObservableCollection<Conteudo_Topico>(myContent_List);
                return TemasList;
            }
        }

        public ObservableCollection<Conteudo_Subtopico> ReadSubtopicos(string idtopico)
        {
            Debug.WriteLine("a procurar subtopico com idtopico=" + idtopico);
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                List<Subtopico> myCollection = dbConn.Query<Subtopico>("select * from Subtopico where ID_Topico = '" + idtopico + "'").ToList<Subtopico>();

                List<Conteudo_Subtopico> myContent_List = new List<Conteudo_Subtopico>();
                foreach (var ts in myCollection)
                {
                    Conteudo_Subtopico myContent = dbConn.Query<Conteudo_Subtopico>("select * from Conteudo_Subtopico WHERE ID_subtopico = '" + ts.Titulo + "' AND Codigo_Idioma =  '" + localSettings.Values["Idioma"] + "';").FirstOrDefault();
                    //Conteudo myContent = dbConn.Query<Conteudo>("select * from Conteudo where id ='" + myContent_lang.id_conteudo + "';").FirstOrDefault();
                    myContent.parent_subtopico = ts;
                    myContent_List.Add(myContent);
                }
                ObservableCollection<Conteudo_Subtopico> TemasList = new ObservableCollection<Conteudo_Subtopico>(myContent_List);
                return TemasList;
            }
        }

        public Boolean SubtopicExist(string idtopico)
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                List<Subtopico> myCollection = dbConn.Query<Subtopico>("select * from Subtopico where ID_Topico = '" + idtopico + "'").ToList<Subtopico>();
                if (myCollection.Count < 1)
                {

                    Debug.WriteLine("a devolver false");
                    return false;
                }
            }
            Debug.WriteLine("a devolver true");
            return true;
        }


        // Insert Stuff into DataBase
        public void Insert(Tema newtema)
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                dbConn.RunInTransaction(() =>
                {
                    dbConn.InsertOrReplace(newtema);
                });
            }
        }

        public void Insert(Topico newtopico)
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                dbConn.RunInTransaction(() =>
                {
                    dbConn.InsertOrReplace(newtopico);
                });
            }
        }

        public void Insert(Conteudo newconteudo)
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                dbConn.RunInTransaction(() =>
                {
                    dbConn.InsertOrReplace(newconteudo);
                });
            }
        }

        public void Insert(Conteudo_Subtopico newconteudo_subtopico)
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                dbConn.RunInTransaction(() =>
                {
                    dbConn.InsertOrReplace(newconteudo_subtopico);
                });
            }
        }

        public void Insert(Conteudo_Tema newconteudo_tema)
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                dbConn.RunInTransaction(() =>
                {
                    dbConn.InsertOrReplace(newconteudo_tema);
                });
            }
        }

        public void Insert(Conteudo_Topico newconteudo_topico)
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                dbConn.RunInTransaction(() =>
                {
                    dbConn.InsertOrReplace(newconteudo_topico);
                });
            }
        }

        public void Insert(Entidade newentidade)
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                dbConn.RunInTransaction(() =>
                {
                    dbConn.InsertOrReplace(newentidade);
                });
            }
        }

        public void Insert(Entidade_Idioma newentidade_idioma)
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                dbConn.RunInTransaction(() =>
                {
                    dbConn.InsertOrReplace(newentidade_idioma);
                });
            }
        }

        public void Insert(Idioma newidioma)
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                dbConn.RunInTransaction(() =>
                {
                    dbConn.InsertOrReplace(newidioma);
                });
            }
        }

        public void Insert(Pais newpais)
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                dbConn.RunInTransaction(() =>
                {
                    dbConn.InsertOrReplace(newpais);
                });
            }
        }

        public void Insert(Palavra newpalavra)
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                dbConn.RunInTransaction(() =>
                {
                    dbConn.InsertOrReplace(newpalavra);
                });
            }
        }

        public void Insert(Subtopico newsubtopico)
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                dbConn.RunInTransaction(() =>
                {
                    dbConn.InsertOrReplace(newsubtopico);
                });
            }
        }

        public void Insert(Tipo_Entidade newtipo_entidade)
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                dbConn.RunInTransaction(() =>
                {
                    dbConn.InsertOrReplace(newtipo_entidade);
                });
            }
        }

        public void Insert(Tipo_Entidade_Idioma newtipo_entidade_idioma)
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                dbConn.RunInTransaction(() =>
                {
                    dbConn.InsertOrReplace(newtipo_entidade_idioma);
                });
            }
        }

        public void Insert(Vertice newvertice)
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                dbConn.RunInTransaction(() =>
                {
                    dbConn.InsertOrReplace(newvertice);
                });
            }
        }



        //Delete Stuff from DB
        public void DeleteAllConteudo()
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                //dbConn.RunInTransaction(() =>
                //   {
                dbConn.DropTable<Conteudo>();
                dbConn.CreateTable<Conteudo>();
                dbConn.Dispose();
                dbConn.Close();
                //});
            }
        }

        public void DeleteAllConteudo_Subtopico()
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                //dbConn.RunInTransaction(() =>
                //   {
                dbConn.DropTable<Conteudo_Subtopico>();
                dbConn.CreateTable<Conteudo_Subtopico>();
                dbConn.Dispose();
                dbConn.Close();
                //});
            }
        }

        public void DeleteAllConteudo_Tema()
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                //dbConn.RunInTransaction(() =>
                //   {
                dbConn.DropTable<Conteudo_Tema>();
                dbConn.CreateTable<Conteudo_Tema>();
                dbConn.Dispose();
                dbConn.Close();
                //});
            }
        }

        public void DeleteAllConteudo_Topico()
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                //dbConn.RunInTransaction(() =>
                //   {
                dbConn.DropTable<Conteudo_Topico>();
                dbConn.CreateTable<Conteudo_Topico>();
                dbConn.Dispose();
                dbConn.Close();
                //});
            }
        }

        public void DeleteAllEntidade()
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                //dbConn.RunInTransaction(() =>
                //   {
                dbConn.DropTable<Entidade>();
                dbConn.CreateTable<Entidade>();
                dbConn.Dispose();
                dbConn.Close();
                //});
            }
        }

        public void DeleteAllEntidade_Idioma()
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                //dbConn.RunInTransaction(() =>
                //   {
                dbConn.DropTable<Entidade_Idioma>();
                dbConn.CreateTable<Entidade_Idioma>();
                dbConn.Dispose();
                dbConn.Close();
                //});
            }
        }

        public void DeleteAllIdioma()
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                //dbConn.RunInTransaction(() =>
                //   {
                dbConn.DropTable<Idioma>();
                dbConn.CreateTable<Idioma>();
                dbConn.Dispose();
                dbConn.Close();
                //});
            }
        }

        public void DeleteAllPais()
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                //dbConn.RunInTransaction(() =>
                //   {
                dbConn.DropTable<Pais>();
                dbConn.CreateTable<Pais>();
                dbConn.Dispose();
                dbConn.Close();
                //});
            }
        }

        public void DeleteAllPalavra()
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                //dbConn.RunInTransaction(() =>
                //   {
                dbConn.DropTable<Palavra>();
                dbConn.CreateTable<Palavra>();
                dbConn.Dispose();
                dbConn.Close();
                //});
            }
        }

        public void DeleteAllSubtopico()
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                //dbConn.RunInTransaction(() =>
                //   {
                dbConn.DropTable<Subtopico>();
                dbConn.CreateTable<Subtopico>();
                dbConn.Dispose();
                dbConn.Close();
                //});
            }
        }

        public void DeleteAllTema()
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                //dbConn.RunInTransaction(() =>
                //   {
                dbConn.DropTable<Tema>();
                dbConn.CreateTable<Tema>();
                dbConn.Dispose();
                dbConn.Close();
                //});
            }
        }

        public void DeleteAllTipo_Entidade()
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                //dbConn.RunInTransaction(() =>
                //   {
                dbConn.DropTable<Tipo_Entidade>();
                dbConn.CreateTable<Tipo_Entidade>();
                dbConn.Dispose();
                dbConn.Close();
                //});
            }
        }

        public void DeleteAllTipo_Entidade_Idioma()
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                //dbConn.RunInTransaction(() =>
                //   {
                dbConn.DropTable<Tipo_Entidade_Idioma>();
                dbConn.CreateTable<Tipo_Entidade_Idioma>();
                dbConn.Dispose();
                dbConn.Close();
                //});
            }
        }

        public void DeleteAllTopico()
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                //dbConn.RunInTransaction(() =>
                //   {
                dbConn.DropTable<Topico>();
                dbConn.CreateTable<Topico>();
                dbConn.Dispose();
                dbConn.Close();
                //});
            }
        }

        public void DeleteAllVertice()
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                //dbConn.RunInTransaction(() =>
                //   {
                dbConn.DropTable<Vertice>();
                dbConn.CreateTable<Vertice>();
                dbConn.Dispose();
                dbConn.Close();
                //});
            }
        }


        //New version, now fixed
        public ObservableCollection<Entidade> ReadEntidade()
        {

            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                List<Entidade> myCollection = dbConn.Table<Entidade>().ToList<Entidade>();

                if ((localSettings.Values["Idioma"] as string) != "pt")
                {
                    foreach (var ent in myCollection)
                    {
                        Entidade_Idioma translatedEnt = dbConn.Query<Entidade_Idioma>("select * from Entidade_Idioma WHERE Nome_Entidade = '" + ent.Nome + "' AND Codigo_Postal_Entidade = '" + ent.Codigo_Postal + "' AND Codigo_Idioma = '" + localSettings.Values["Idioma"] + "';").FirstOrDefault();
                        Tipo_Entidade_Idioma translatedEntType = dbConn.Query<Tipo_Entidade_Idioma>("select * from Tipo_Entidade_Idioma WHERE Nome_Tipo_Entidade = '" + ent.Nome_Tipo_Entidade + "' AND Codigo_Idioma = '" + localSettings.Values["Idioma"] + "';").FirstOrDefault();
                        if (translatedEnt != null)
                            ent.Nome = translatedEnt.Nome_Idioma;
                        if (translatedEntType != null)
                            ent.Nome_Tipo_Entidade = translatedEntType.Nome_Idioma;
                    }
                }
                ObservableCollection<Entidade> EntList = new ObservableCollection<Entidade>(myCollection);
                return EntList;
            }
        }

         public ObservableCollection<Tipo_Entidade_Idioma> ReadTipoEntidade()
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                List<Tipo_Entidade_Idioma> myCollection = new List<Tipo_Entidade_Idioma>();
                if ((localSettings.Values["Idioma"] as string) != "pt")
                {
                    myCollection = dbConn.Query<Tipo_Entidade_Idioma>("select * from Tipo_Entidade_Idioma where Codigo_Idioma = '" + localSettings.Values["Idioma"] + "';").ToList<Tipo_Entidade_Idioma>();
                }
                else
                {
                    List<Tipo_Entidade> mytempCollection = dbConn.Query<Tipo_Entidade>("select * from Tipo_Entidade;").ToList<Tipo_Entidade>();
                    foreach (var ent in mytempCollection)
                    {
                        myCollection.Add(new Tipo_Entidade_Idioma("pt", ent.Nome, ent.Nome));
                    }
                }
                ObservableCollection<Tipo_Entidade_Idioma> EntList = new ObservableCollection<Tipo_Entidade_Idioma>(myCollection);
                return EntList;
            }
        }

        public double latitude(int N_Vertice)
        {

            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                var vert = dbConn.Query<Vertice>("select * from Vertice where N_Vertice =" + N_Vertice).FirstOrDefault();
                return vert.Latitude;
            }
        }

        public double longitude(int N_Vertice)
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                var vert = dbConn.Query<Vertice>("select * from Vertice where N_Vertice =" + N_Vertice).FirstOrDefault();
                return vert.Longitude;
            }
        }

        public ObservableCollection<Idioma> ReadIdiomas()
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                List<Idioma> myCollection = dbConn.Table<Idioma>().ToList<Idioma>();
                ObservableCollection<Idioma> myLang = new ObservableCollection<Idioma>(myCollection);
                return myLang;
            }
        }

        public ObservableCollection<Pais> ReadPaises()
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                List<Pais> myCollection = dbConn.Table<Pais>().ToList<Pais>();
                ObservableCollection<Pais> myCount = new ObservableCollection<Pais>(myCollection);
                return myCount;
            }
        }

        public string Translate(string s)
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                Palavra p = dbConn.Query<Palavra>("select * from Palavra where Frase = '" + s + "';").FirstOrDefault();
                Palavra tp = dbConn.Query<Palavra>("select * from Palavra where Codigo_Ascendente =" + p.Codigo_Frase + " AND Codigo_Idioma = '" + localSettings.Values["Idioma"] + "';").FirstOrDefault();
                return tp.Frase;
            }
        }


    }
}
