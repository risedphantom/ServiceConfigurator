using Newtonsoft.Json.Linq;

namespace JiraTfsWebApi.Lib
{
    static class JsonTransformer
    {
        public static object PrepareForOzon(JObject issueObject)
        {
            switch (issueObject["fields"]["customfield_10101"]["child"]["value"].ToString())
            {
                #region -=Установка или настройка принтера=-
                case "Установка или настройка принтера":
                    return new
                    {
                        fields = new
                        {
                            project = new
                            {
                                key = "SUPPORT"
                            },
                            issuetype = new
                            {
                                name = "Заявка"
                            },
                            // Editable part
                            customfield_13100 = new
                            {
                                value = "Интернет Трэвел",
                                child = new
                                {
                                    value = "Москва - Чапаевский"
                                }
                            },
                            customfield_11103 = new
                            {
                                value = "Принтеры/Офисная техника",
                                child = new
                                {
                                    value = "Установка или настройка принтера"
                                }
                            },
                            summary = issueObject["fields"]["summary"].ToString(),
                            customfield_13302 = issueObject["fields"]["customfield_10102"].ToString(),
                            customfield_13303 = issueObject["fields"]["description"].ToString(),
                            customfield_13301 = issueObject["fields"]["customfield_10100"].ToString(),
                            description = issueObject["fields"]["customfield_10105"].ToString()
                        }
                    };
                #endregion
                #region -=Предоставление/продление удаленного доступа=-
                case "Предоставление/продление удаленного доступа":
                    return null;
                #endregion
            }
            return null;
        }
    }
}
