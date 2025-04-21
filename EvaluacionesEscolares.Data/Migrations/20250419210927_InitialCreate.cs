using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EvaluacionesEscolares.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Estudiantes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Apellido = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Matricula = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaNacimiento = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Estudiantes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Materias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Materias", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Examenes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titulo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaAplicacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DuracionMinutos = table.Column<int>(type: "int", nullable: false),
                    MateriaId = table.Column<int>(type: "int", nullable: false),
                    Tipo = table.Column<int>(type: "int", nullable: false),
                    Duracion = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Examenes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Examenes_Materias_MateriaId",
                        column: x => x.MateriaId,
                        principalTable: "Materias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Calificaciones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nota = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EstudianteId = table.Column<int>(type: "int", nullable: false),
                    ExamenId = table.Column<int>(type: "int", nullable: false),
                    Comentarios = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Calificaciones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Calificaciones_Estudiantes_EstudianteId",
                        column: x => x.EstudianteId,
                        principalTable: "Estudiantes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Calificaciones_Examenes_ExamenId",
                        column: x => x.ExamenId,
                        principalTable: "Examenes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Preguntas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Texto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Puntos = table.Column<int>(type: "int", nullable: false),
                    ExamenId = table.Column<int>(type: "int", nullable: false),
                    TextoPregunta = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Preguntas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Preguntas_Examenes_ExamenId",
                        column: x => x.ExamenId,
                        principalTable: "Examenes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Opciones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Texto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EsCorrecta = table.Column<bool>(type: "bit", nullable: false),
                    PreguntaId = table.Column<int>(type: "int", nullable: false),
                    TextoOpcion = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Opciones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Opciones_Preguntas_PreguntaId",
                        column: x => x.PreguntaId,
                        principalTable: "Preguntas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Materias",
                columns: new[] { "Id", "Descripcion", "Nombre" },
                values: new object[,]
                {
                    { 1, "Estudio del idioma español y su literatura", "Lengua Española" },
                    { 2, "Estudio de números, cantidades y formas", "Matemática" },
                    { 3, "Estudio de los seres vivos", "Biología" },
                    { 4, "Estudio de la materia y la energía", "Física" },
                    { 5, "Estudio de la composición de la materia", "Química" },
                    { 6, "Estudio de la Tierra y sus características", "Geografía" },
                    { 7, "Estudio de eventos pasados", "Historia" },
                    { 8, "Desarrollo físico y deportivo", "Educación Física" },
                    { 9, "Estudio de idiomas extranjeros", "Lengua Extranjera" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Calificaciones_EstudianteId",
                table: "Calificaciones",
                column: "EstudianteId");

            migrationBuilder.CreateIndex(
                name: "IX_Calificaciones_ExamenId",
                table: "Calificaciones",
                column: "ExamenId");

            migrationBuilder.CreateIndex(
                name: "IX_Examenes_MateriaId",
                table: "Examenes",
                column: "MateriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Opciones_PreguntaId",
                table: "Opciones",
                column: "PreguntaId");

            migrationBuilder.CreateIndex(
                name: "IX_Preguntas_ExamenId",
                table: "Preguntas",
                column: "ExamenId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Calificaciones");

            migrationBuilder.DropTable(
                name: "Opciones");

            migrationBuilder.DropTable(
                name: "Estudiantes");

            migrationBuilder.DropTable(
                name: "Preguntas");

            migrationBuilder.DropTable(
                name: "Examenes");

            migrationBuilder.DropTable(
                name: "Materias");
        }
    }
}
