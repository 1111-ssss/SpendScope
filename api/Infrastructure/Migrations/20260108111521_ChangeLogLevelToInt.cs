using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeLogLevelToInt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_profiles_users_user_id",
                table: "profiles");

            migrationBuilder.DropForeignKey(
                name: "fk_user_achievements_achievements_achievement_id",
                table: "user_achievements");

            migrationBuilder.DropPrimaryKey(
                name: "pk_users",
                table: "users");

            migrationBuilder.DropPrimaryKey(
                name: "pk_user_achievements",
                table: "user_achievements");

            migrationBuilder.DropPrimaryKey(
                name: "pk_profiles",
                table: "profiles");

            migrationBuilder.DropPrimaryKey(
                name: "pk_follows",
                table: "follows");

            migrationBuilder.DropPrimaryKey(
                name: "pk_app_versions",
                table: "app_versions");

            migrationBuilder.DropPrimaryKey(
                name: "pk_achievements",
                table: "achievements");

            migrationBuilder.RenameIndex(
                name: "ix_users_username",
                table: "users",
                newName: "users_username_key");

            migrationBuilder.RenameIndex(
                name: "ix_users_email",
                table: "users",
                newName: "users_email_key");

            migrationBuilder.RenameIndex(
                name: "ix_user_achievements_user_id",
                table: "user_achievements",
                newName: "IX_user_achievements_user_id");

            migrationBuilder.RenameIndex(
                name: "ix_user_achievements_achievement_id",
                table: "user_achievements",
                newName: "IX_user_achievements_achievement_id");

            migrationBuilder.RenameIndex(
                name: "ix_profiles_display_name",
                table: "profiles",
                newName: "IX_profiles_display_name");

            migrationBuilder.RenameIndex(
                name: "ix_follows_follower_id",
                table: "follows",
                newName: "idx_follows_follower");

            migrationBuilder.RenameIndex(
                name: "ix_follows_followed_id",
                table: "follows",
                newName: "idx_follows_followed");

            migrationBuilder.RenameIndex(
                name: "ix_app_versions_uploaded_by",
                table: "app_versions",
                newName: "IX_app_versions_uploaded_by");

            migrationBuilder.RenameIndex(
                name: "ix_app_versions_uploaded_at",
                table: "app_versions",
                newName: "IX_app_versions_uploaded_at");

            migrationBuilder.RenameIndex(
                name: "ix_achievements_name",
                table: "achievements",
                newName: "achievements_name_key");

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                table: "users",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "users",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<DateTime>(
                name: "unlocked_at",
                table: "user_achievements",
                type: "timestamp with time zone",
                nullable: true,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true,
                oldDefaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AddColumn<int>(
                name: "AchievementId1",
                table: "user_achievements",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "last_online",
                table: "profiles",
                type: "timestamp with time zone",
                nullable: true,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true,
                oldDefaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AlterColumn<DateTime>(
                name: "followed_at",
                table: "follows",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AlterColumn<DateTime>(
                name: "uploaded_at",
                table: "app_versions",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AddPrimaryKey(
                name: "PK_users",
                table: "users",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_user_achievements",
                table: "user_achievements",
                columns: new[] { "user_id", "achievement_id" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_profiles",
                table: "profiles",
                column: "user_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_follows",
                table: "follows",
                columns: new[] { "follower_id", "followed_id" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_app_versions",
                table: "app_versions",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_achievements",
                table: "achievements",
                column: "id");

            migrationBuilder.CreateTable(
                name: "log_entries",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    level = table.Column<int>(type: "integer", nullable: false),
                    message = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    exception = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_log_entries", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_user_achievements_AchievementId1",
                table: "user_achievements",
                column: "AchievementId1");

            migrationBuilder.AddForeignKey(
                name: "FK_profiles_users_user_id",
                table: "profiles",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_user_achievements_achievements_AchievementId1",
                table: "user_achievements",
                column: "AchievementId1",
                principalTable: "achievements",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_profiles_users_user_id",
                table: "profiles");

            migrationBuilder.DropForeignKey(
                name: "FK_user_achievements_achievements_AchievementId1",
                table: "user_achievements");

            migrationBuilder.DropTable(
                name: "log_entries");

            migrationBuilder.DropPrimaryKey(
                name: "PK_users",
                table: "users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_user_achievements",
                table: "user_achievements");

            migrationBuilder.DropIndex(
                name: "IX_user_achievements_AchievementId1",
                table: "user_achievements");

            migrationBuilder.DropPrimaryKey(
                name: "PK_profiles",
                table: "profiles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_follows",
                table: "follows");

            migrationBuilder.DropPrimaryKey(
                name: "PK_app_versions",
                table: "app_versions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_achievements",
                table: "achievements");

            migrationBuilder.DropColumn(
                name: "AchievementId1",
                table: "user_achievements");

            migrationBuilder.RenameIndex(
                name: "users_username_key",
                table: "users",
                newName: "ix_users_username");

            migrationBuilder.RenameIndex(
                name: "users_email_key",
                table: "users",
                newName: "ix_users_email");

            migrationBuilder.RenameIndex(
                name: "IX_user_achievements_user_id",
                table: "user_achievements",
                newName: "ix_user_achievements_user_id");

            migrationBuilder.RenameIndex(
                name: "IX_user_achievements_achievement_id",
                table: "user_achievements",
                newName: "ix_user_achievements_achievement_id");

            migrationBuilder.RenameIndex(
                name: "IX_profiles_display_name",
                table: "profiles",
                newName: "ix_profiles_display_name");

            migrationBuilder.RenameIndex(
                name: "idx_follows_follower",
                table: "follows",
                newName: "ix_follows_follower_id");

            migrationBuilder.RenameIndex(
                name: "idx_follows_followed",
                table: "follows",
                newName: "ix_follows_followed_id");

            migrationBuilder.RenameIndex(
                name: "IX_app_versions_uploaded_by",
                table: "app_versions",
                newName: "ix_app_versions_uploaded_by");

            migrationBuilder.RenameIndex(
                name: "IX_app_versions_uploaded_at",
                table: "app_versions",
                newName: "ix_app_versions_uploaded_at");

            migrationBuilder.RenameIndex(
                name: "achievements_name_key",
                table: "achievements",
                newName: "ix_achievements_name");

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                table: "users",
                type: "timestamp without time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "users",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<DateTime>(
                name: "unlocked_at",
                table: "user_achievements",
                type: "timestamp without time zone",
                nullable: true,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldDefaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AlterColumn<DateTime>(
                name: "last_online",
                table: "profiles",
                type: "timestamp without time zone",
                nullable: true,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldDefaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AlterColumn<DateTime>(
                name: "followed_at",
                table: "follows",
                type: "timestamp without time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AlterColumn<DateTime>(
                name: "uploaded_at",
                table: "app_versions",
                type: "timestamp without time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AddPrimaryKey(
                name: "pk_users",
                table: "users",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_user_achievements",
                table: "user_achievements",
                columns: new[] { "user_id", "achievement_id" });

            migrationBuilder.AddPrimaryKey(
                name: "pk_profiles",
                table: "profiles",
                column: "user_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_follows",
                table: "follows",
                columns: new[] { "follower_id", "followed_id" });

            migrationBuilder.AddPrimaryKey(
                name: "pk_app_versions",
                table: "app_versions",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_achievements",
                table: "achievements",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_profiles_users_user_id",
                table: "profiles",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_user_achievements_achievements_achievement_id",
                table: "user_achievements",
                column: "achievement_id",
                principalTable: "achievements",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
