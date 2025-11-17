namespace rise_gs.DTOs
{
    public class BemEstarDto
    {
        public int IdBemEstar { get; set; }
        public DateTime DtRegistro { get; set; }
        public int NivelHumor { get; set; }
        public TimeSpan? HorasEstudo { get; set; }
        public string? DescAtividade { get; set; }

        public int IdUsuario { get; set; }
    }
}
