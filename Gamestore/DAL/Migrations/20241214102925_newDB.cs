using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DAL.Migrations;

/// <inheritdoc />
public partial class NewDB : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Games",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Key = table.Column<string>(type: "nvarchar(450)", nullable: false),
                Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Games", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Genres",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                ParentGenreId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Genres", x => x.Id);
                table.ForeignKey(
                    name: "FK_Genres_Genres_ParentGenreId",
                    column: x => x.ParentGenreId,
                    principalTable: "Genres",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "Platforms",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Type = table.Column<string>(type: "nvarchar(450)", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Platforms", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "GameGenre",
            columns: table => new
            {
                GamesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                GenresId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_GameGenre", x => new { x.GamesId, x.GenresId });
                table.ForeignKey(
                    name: "FK_GameGenre_Games_GamesId",
                    column: x => x.GamesId,
                    principalTable: "Games",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_GameGenre_Genres_GenresId",
                    column: x => x.GenresId,
                    principalTable: "Genres",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "GamePlatform",
            columns: table => new
            {
                GamesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                PlatformsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_GamePlatform", x => new { x.GamesId, x.PlatformsId });
                table.ForeignKey(
                    name: "FK_GamePlatform_Games_GamesId",
                    column: x => x.GamesId,
                    principalTable: "Games",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_GamePlatform_Platforms_PlatformsId",
                    column: x => x.PlatformsId,
                    principalTable: "Platforms",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.InsertData(
            table: "Genres",
            columns: ["Id", "Name", "ParentGenreId"],
            values: new object[,]
            {
                { new Guid("3768cb72-a96b-4894-ad41-5ba15c8f4307"), "Action", null },
                { new Guid("3dd316d1-fa85-455e-a21d-b367f12635a3"), "Puzzle & Skill", null },
                { new Guid("46165ae2-b7b5-4147-a6cf-bf1d19611b7f"), "Strategy", null },
                { new Guid("96a88ced-7d69-42c2-a658-6c6280ab9bb1"), "Adventure", null },
                { new Guid("b33bbde2-0a2e-499e-b70f-bb6879e38795"), "Sports", null },
                { new Guid("def411c6-6ca6-4506-9a3a-34ed2999b9d5"), "RPG", null },
                { new Guid("01d77623-f0be-47ab-9446-8bcc5c8650b9"), "FPS", new Guid("3768cb72-a96b-4894-ad41-5ba15c8f4307") },
                { new Guid("26be97b2-40f4-4a37-8237-1c65e2ab6813"), "Races", new Guid("b33bbde2-0a2e-499e-b70f-bb6879e38795") },
                { new Guid("26e0605a-260f-430c-86af-154bfee66e58"), "RTS", new Guid("46165ae2-b7b5-4147-a6cf-bf1d19611b7f") },
                { new Guid("5266fca4-4016-47dd-9542-174f53c43b1a"), "TPS", new Guid("3768cb72-a96b-4894-ad41-5ba15c8f4307") },
                { new Guid("e1250942-0bfe-46fb-9059-f9cc2e79428a"), "TBS", new Guid("46165ae2-b7b5-4147-a6cf-bf1d19611b7f") },
                { new Guid("26be36b2-cceb-4222-84ce-aaa9f3ff5d2c"), "Off-road", new Guid("26be97b2-40f4-4a37-8237-1c65e2ab6813") },
                { new Guid("5598feb0-6a7c-4a04-95c6-5f126ac69b68"), "Formula", new Guid("26be97b2-40f4-4a37-8237-1c65e2ab6813") },
                { new Guid("bf0dfa8f-27d5-41ad-8e07-b2f6e5e39061"), "Rally", new Guid("26be97b2-40f4-4a37-8237-1c65e2ab6813") },
                { new Guid("dbbf13cd-ddd3-4b0b-9c49-5474ccae051d"), "Arcade", new Guid("26be97b2-40f4-4a37-8237-1c65e2ab6813") },
            });

        migrationBuilder.CreateIndex(
            name: "IX_GameGenre_GenresId",
            table: "GameGenre",
            column: "GenresId");

        migrationBuilder.CreateIndex(
            name: "IX_GamePlatform_PlatformsId",
            table: "GamePlatform",
            column: "PlatformsId");

        migrationBuilder.CreateIndex(
            name: "IX_Games_Key",
            table: "Games",
            column: "Key",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Genres_Name",
            table: "Genres",
            column: "Name",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Genres_ParentGenreId",
            table: "Genres",
            column: "ParentGenreId");

        migrationBuilder.CreateIndex(
            name: "IX_Platforms_Type",
            table: "Platforms",
            column: "Type",
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "GameGenre");

        migrationBuilder.DropTable(
            name: "GamePlatform");

        migrationBuilder.DropTable(
            name: "Genres");

        migrationBuilder.DropTable(
            name: "Games");

        migrationBuilder.DropTable(
            name: "Platforms");
    }
}
