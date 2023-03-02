using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace WorkerConecMongo
{
    class Wap_IngresosPedidosContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //"Server=sql_server2022;Database=SalesDb;User Id=SA;Password=A&VeryComplex123Password;MultipleActiveResultSets=true"
            //optionsBuilder.UseSqlServer("Server=DBINTERFWHSDESA;Database=InterfacesWHS;User Id=User_InterfacesWHS;Password=0jIN7HD9BlT6cnGoKZYa;MultipleActiveResultSets=true;");
            //optionsBuilder.UseSqlServer("Data Source=DBINTERFWHSDESA;Initial Catalog=InterfacesWHS;User ID=User_InterfacesWHS;Password=0jIN7HD9BlT6cnGoKZYa;Integrated Security=False;");
            //optionsBuilder.UseSqlServer(@"Data Source=SQLSCECYPESRV.andreani.com.ar;Initial catalog=InterfacesWHS;Integrated Security=true");
            //"Data Source=%DNS%.andreani.com.ar;Initial Catalog=%base%;Persist Security Info=True;User ID=%User%;Password=%Password%"
                  optionsBuilder.UseSqlServer(@"Data Source=DBINTERFWHSDESA.andreani.com.ar;Initial catalog=InterfacesWHS;User ID=User_InterfacesWHS;Password=0jIN7HD9BlT6cnGoKZYa;");
            //optionsBuilder.UseSqlServer(@"Data Source=SQLSCECYPESRV;Initial catalog=InterfacesWHS;Integrated Security=true");
            //optionsBuilder.UseSqlServer(@"Data Source=ITGDESAOCSRV;Initial catalog=AccionesBBVA;Integrated Security=true");
        }

        public DbSet<WAP_INGRESOPEDIDOS> WAP_INGRESOPEDIDOS { get; set; }
    }
}
