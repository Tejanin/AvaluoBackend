namespace AvaluoAPI.Presentation.ViewModels
{
    public class RolViewModel
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public bool EsProfesor { get; set; }
        public bool EsSupervisor { get; set; }
        public bool EsCoordinadorArea { get; set; }
        public bool EsCoordinadorCarrera { get; set; }
        public bool EsAdmin { get; set; }
        public bool EsAux { get; set; }
        public bool VerInformes { get; set; }
        public bool VerListaDeRubricas { get; set; }
        public bool ConfigurarFechas { get; set; }
        public bool VerManejoCurriculum { get; set; }
    }
}
