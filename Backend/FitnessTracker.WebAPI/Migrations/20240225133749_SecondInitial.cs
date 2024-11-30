using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessTracker.WebAPI.Migrations
{
    public partial class SecondInitial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActivityDuration",
                table: "AerobicTrainings");

            migrationBuilder.AddColumn<int>(
                name: "ActivityDurationMinutes",
                table: "AerobicTrainings",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActivityDurationMinutes",
                table: "AerobicTrainings");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "ActivityDuration",
                table: "AerobicTrainings",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }
    }
}
