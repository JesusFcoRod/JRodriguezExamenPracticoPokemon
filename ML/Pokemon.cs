namespace ML
{
    public class Pokemon
    {
        public string Nombre { get; set; }
        public string Imagen { get; set; }
        public string url { get; set; }

        public string Tipo { get; set; }
        public string TipoUrl { get; set; }
        public string Habilidad { get; set; }
        public int Experiencia { get; set; }

        public List<object> Pokemones { get; set; }
    }
}