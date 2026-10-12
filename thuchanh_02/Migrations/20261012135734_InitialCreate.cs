using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace thuchanh_02.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:auth.aal_level", "aal1,aal2,aal3")
                .Annotation("Npgsql:Enum:auth.code_challenge_method", "s256,plain")
                .Annotation("Npgsql:Enum:auth.factor_status", "unverified,verified")
                .Annotation("Npgsql:Enum:auth.factor_type", "totp,webauthn,phone,recovery_code")
                .Annotation("Npgsql:Enum:auth.oauth_authorization_status", "pending,approved,denied,expired")
                .Annotation("Npgsql:Enum:auth.oauth_client_type", "public,confidential")
                .Annotation("Npgsql:Enum:auth.oauth_registration_type", "dynamic,manual")
                .Annotation("Npgsql:Enum:auth.oauth_response_type", "code")
                .Annotation("Npgsql:Enum:auth.one_time_token_type", "confirmation_token,reauthentication_token,recovery_token,email_change_token_new,email_change_token_current,phone_change_token")
                .Annotation("Npgsql:Enum:realtime.action", "INSERT,UPDATE,DELETE,TRUNCATE,ERROR")
                .Annotation("Npgsql:Enum:realtime.equality_op", "eq,neq,lt,lte,gt,gte,in,like,ilike,is,match,imatch,isdistinct")
                .Annotation("Npgsql:Enum:storage.buckettype", "STANDARD,ANALYTICS,VECTOR")
                .Annotation("Npgsql:PostgresExtension:extensions.pg_stat_statements", ",,")
                .Annotation("Npgsql:PostgresExtension:extensions.pgcrypto", ",,")
                .Annotation("Npgsql:PostgresExtension:extensions.uuid-ossp", ",,")
                .Annotation("Npgsql:PostgresExtension:vault.supabase_vault", ",,");

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    IdUser = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Password = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Token = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("User_pkey", x => x.IdUser);
                });

            migrationBuilder.CreateIndex(
                name: "User_UserName_key",
                table: "User",
                column: "UserName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
