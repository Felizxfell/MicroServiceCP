using Dapper;
using MicroServiceCP.Entidades;
using MySql.Data.MySqlClient;

namespace MicroServiceCP.Data
{
    public class TablaClientePagosAnterior
    {
        private readonly string con;

        public TablaClientePagosAnterior(MysqlConfiguration connection)
        {
            con = connection.ConnectionString;
        }

        /// <summary>
        /// Metodo que nos da acceso a la conexion de la base de datos
        /// </summary>
        /// <returns></returns>
        protected MySqlConnection DbConnection()
        {
            return new MySqlConnection(con);
        }

        /// <summary>
        /// Inserta los pagos del dia anterior
        /// </summary>
        /// <param name="pago"></param>
        /// <returns></returns>
        public async Task<bool> InsertPagoAnterior(string diaAnterior)
        {
            var db = DbConnection();

            string sql = @"SELECT id AS PagoId, cliente_id AS ClienteId, fecha_creacion
                        FROM pagos
                        WHERE fecha_creacion BETWEEN @Rango1 AND @Rango2 ";

            var ids = await db.QueryAsync<ClientesPagosAnterior>(sql, new { Rango1 = Convert.ToDateTime($"{diaAnterior} 00:00:00"), Rango2 = Convert.ToDateTime($"{diaAnterior} 23:59:59") });

            if (ids.Count() == 0)
                return false;

            string sql2 = @"INSERT INTO clientes_pagos_anterior (cliente_id, pago_id) VALUES ";

            int i = 0;
            foreach (var id in ids)
            {
                if (i == 0) 
                    sql2 += $@"({id.ClienteId}, {id.PagoId}) ";
                else
                    sql2 += $@", ({id.ClienteId}, {id.PagoId})";
                i++;
            }            

            var result = await db.ExecuteAsync(sql2, new { });

            return result > 0;
        }

        /// <summary>
        /// Obtiene todos los clientes mas sus pagos del dia anterior
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<ClientesPagosAntResponse>> GetAllPaymentsAnterior()
        {
            var db = DbConnection();

            var sql = @"SELECT cpa.cliente_id AS ClienteId, c.nombre, cpa.pago_id AS PagoId, p.monto, p.fecha_creacion AS FechaPago
                        FROM clientes_pagos_anterior AS cpa
                        INNER JOIN clientes AS c ON cpa.cliente_id = c.id
                        INNER JOIN pagos AS p ON cpa.pago_id = p.id";

            return await db.QueryAsync<ClientesPagosAntResponse>(sql, new { });
        }
    }
}
