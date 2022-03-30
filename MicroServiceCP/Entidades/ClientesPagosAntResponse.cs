namespace MicroServiceCP.Entidades
{
    public class ClientesPagosAntResponse
    {
        public int ClienteId { get; set; }
        
        public string Nombre { get; set; }

        public int PagoId { get; set; }

        public double Monto { get; set; }

        public DateTime FechaPago { get; set; }
    }
}
