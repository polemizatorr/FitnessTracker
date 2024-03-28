using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessTracker.WebAPI.Migrations
{
    public partial class InitialCreat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModificationTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "AerobicTrainings",
                columns: table => new
                {
                    AerobicTrainingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ActivityType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ActivityDuration = table.Column<TimeSpan>(type: "time", nullable: false),
                    CalorieBurnt = table.Column<int>(type: "int", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModificationTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AerobicTrainings", x => x.AerobicTrainingId);
                    table.ForeignKey(
                        name: "FK_AerobicTrainings_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StrenghtTrainings",
                columns: table => new
                {
                    StrenghtTrainingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModificationTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StrenghtTrainings", x => x.StrenghtTrainingId);
                    table.ForeignKey(
                        name: "FK_StrenghtTrainings_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sets",
                columns: table => new
                {
                    SetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StrenghtTrainingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RepetitionsNumber = table.Column<int>(type: "int", nullable: false),
                    ExerciseName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExhaustionLevel = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sets", x => x.SetId);
                    table.ForeignKey(
                        name: "FK_Sets_StrenghtTrainings_StrenghtTrainingId",
                        column: x => x.StrenghtTrainingId,
                        principalTable: "StrenghtTrainings",
                        principalColumn: "StrenghtTrainingId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AerobicTrainings_UserId",
                table: "AerobicTrainings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Sets_StrenghtTrainingId",
                table: "Sets",
                column: "StrenghtTrainingId");

            migrationBuilder.CreateIndex(
                name: "IX_StrenghtTrainings_UserId",
                table: "StrenghtTrainings",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AerobicTrainings");

            migrationBuilder.DropTable(
                name: "Sets");

            migrationBuilder.DropTable(
                name: "StrenghtTrainings");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
