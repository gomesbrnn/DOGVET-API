using Microsoft.EntityFrameworkCore.Migrations;

namespace App.Migrations
{
    public partial class PopulaDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO `autenticacoes` VALUES (1,'funcionario@gft.com','funcionario',1,1),(2,'cliente@gft.com','cliente',1,0)");
            migrationBuilder.Sql("INSERT INTO `clinicas` VALUES (1,'Dog Vet','93407096000198','Rua da Moeda, Recife-PE',1)");
            migrationBuilder.Sql("INSERT INTO `veterinarios` VALUES (1,'Carlos','99390254086',1),(2,'Diego','48953256011',1)");
            migrationBuilder.Sql("INSERT INTO `tutores` VALUES (1,'André','47007585035',0,1),(2,'Clécio','07080998077',0,1)");
            migrationBuilder.Sql("INSERT INTO `animais` VALUES (1,'Chico','Bulldog','4kg','2022-01-15 00:00:00.000000',1,1),(2,'Chivo','Pug','3kg','2022-02-04 00:00:00.000000',1,1),(3,'Vodka','Pit Bull','6kg','2022-01-25 00:00:00.000000',2,1),(4,'Zeus','Labrador','5kg','2022-03-05 00:00:00.000000',2,1)");
            migrationBuilder.Sql("INSERT INTO `atendimentos` VALUES (1,1,1,1,1,'2022-07-22 14:29:43.632000','O Peso se manteve','Sedentarismo, dificuldades respiratórias','Recomendado atividades de caminhada e corridas com o cachorro',1),(2,1,1,1,2,'2022-07-22 15:01:56.015000','Peso: Aumento de 3kg para 4Kg','O cachorro se apresenta saudável','Manter dieta e peso atual do cachorro, ambos estão nas medidas corretas',1),(3,1,2,2,3,'2022-07-22 15:08:08.324000','Peso: Aumentou de 6kg para 7kg','Infeção por corte na pata posterior direita','Peso do cachorro ok, tratamento com remédio anti-inflamatório por 7 dias',1),(4,1,2,2,4,'2022-07-22 15:08:08.324000','Peso: baixou de 5kg para 4kg','Fastio causado por ansiedade de separação','Recomendado realizar mais atividades com seu cachorro e incentivá-lo a comer após elas.',1)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM autenticacoes");
            migrationBuilder.Sql("DELETE FROM clinicas");
            migrationBuilder.Sql("DELETE FROM veterinarios");
            migrationBuilder.Sql("DELETE FROM tutores");
            migrationBuilder.Sql("DELETE FROM animais");
            migrationBuilder.Sql("DELETE FROM atendimentos");
        }
    }
}
