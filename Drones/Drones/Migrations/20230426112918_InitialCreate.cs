using Microsoft.EntityFrameworkCore.Migrations;

namespace Drones.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Drones",
                columns: table => new
                {
                    DroneId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SN = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Model = table.Column<int>(type: "INTEGER", nullable: false),
                    WeightLimit = table.Column<int>(type: "INTEGER", nullable: false),
                    BatteryCapacity = table.Column<int>(type: "INTEGER", nullable: false),
                    State = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Drones", x => x.DroneId);
                });

            migrationBuilder.CreateTable(
                name: "Medications",
                columns: table => new
                {
                    MedicationId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Weight = table.Column<int>(type: "INTEGER", nullable: false),
                    Code = table.Column<string>(type: "TEXT", nullable: false),
                    Image = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medications", x => x.MedicationId);
                });

            migrationBuilder.CreateTable(
                name: "DroneMedication",
                columns: table => new
                {
                    DroneId = table.Column<int>(type: "INTEGER", nullable: false),
                    MedicationsMedicationId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DroneMedication", x => new { x.DroneId, x.MedicationsMedicationId });
                    table.ForeignKey(
                        name: "FK_DroneMedication_Drones_DroneId",
                        column: x => x.DroneId,
                        principalTable: "Drones",
                        principalColumn: "DroneId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DroneMedication_Medications_MedicationsMedicationId",
                        column: x => x.MedicationsMedicationId,
                        principalTable: "Medications",
                        principalColumn: "MedicationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DroneMedication_MedicationsMedicationId",
                table: "DroneMedication",
                column: "MedicationsMedicationId");

            migrationBuilder.CreateIndex(
                name: "IX_Drones_SN",
                table: "Drones",
                column: "SN",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Medications_Name",
                table: "Medications",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DroneMedication");

            migrationBuilder.DropTable(
                name: "Drones");

            migrationBuilder.DropTable(
                name: "Medications");
        }
    }
}
