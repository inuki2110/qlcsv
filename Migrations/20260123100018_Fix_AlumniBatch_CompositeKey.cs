using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QLCSV.Migrations
{
    /// <inheritdoc />
    public partial class Fix_AlumniBatch_CompositeKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_alumni_batches_alumni_profiles_AlumniId",
                table: "alumni_batches");

            migrationBuilder.DropForeignKey(
                name: "FK_alumni_batches_batches_BatchId",
                table: "alumni_batches");

            migrationBuilder.DropPrimaryKey(
                name: "PK_batches",
                table: "batches");

            migrationBuilder.DropIndex(
                name: "IX_batches_GraduationYear_Name",
                table: "batches");

            migrationBuilder.DropPrimaryKey(
                name: "PK_alumni_batches",
                table: "alumni_batches");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "faculties");

            migrationBuilder.RenameTable(
                name: "batches",
                newName: "Batches");

            migrationBuilder.RenameTable(
                name: "alumni_batches",
                newName: "AlumniBatches");

            migrationBuilder.RenameIndex(
                name: "IX_alumni_batches_BatchId",
                table: "AlumniBatches",
                newName: "IX_AlumniBatches_BatchId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "users",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "users",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Batches",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<bool>(
                name: "IsPublic",
                table: "alumni_profiles",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Batches",
                table: "Batches",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AlumniBatches",
                table: "AlumniBatches",
                columns: new[] { "AlumniId", "BatchId" });

            migrationBuilder.AddForeignKey(
                name: "FK_AlumniBatches_Batches_BatchId",
                table: "AlumniBatches",
                column: "BatchId",
                principalTable: "Batches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AlumniBatches_alumni_profiles_AlumniId",
                table: "AlumniBatches",
                column: "AlumniId",
                principalTable: "alumni_profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AlumniBatches_Batches_BatchId",
                table: "AlumniBatches");

            migrationBuilder.DropForeignKey(
                name: "FK_AlumniBatches_alumni_profiles_AlumniId",
                table: "AlumniBatches");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Batches",
                table: "Batches");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AlumniBatches",
                table: "AlumniBatches");

            migrationBuilder.RenameTable(
                name: "Batches",
                newName: "batches");

            migrationBuilder.RenameTable(
                name: "AlumniBatches",
                newName: "alumni_batches");

            migrationBuilder.RenameIndex(
                name: "IX_AlumniBatches_BatchId",
                table: "alumni_batches",
                newName: "IX_alumni_batches_BatchId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "users",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "users",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "faculties",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "batches",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<bool>(
                name: "IsPublic",
                table: "alumni_profiles",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AddPrimaryKey(
                name: "PK_batches",
                table: "batches",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_alumni_batches",
                table: "alumni_batches",
                columns: new[] { "AlumniId", "BatchId" });

            migrationBuilder.CreateIndex(
                name: "IX_batches_GraduationYear_Name",
                table: "batches",
                columns: new[] { "GraduationYear", "Name" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_alumni_batches_alumni_profiles_AlumniId",
                table: "alumni_batches",
                column: "AlumniId",
                principalTable: "alumni_profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_alumni_batches_batches_BatchId",
                table: "alumni_batches",
                column: "BatchId",
                principalTable: "batches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
