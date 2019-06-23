using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Inventory.Web.Migrations
{
    public partial class CreateFullView : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE VIEW View_ItemFullView AS
                select I.*,
                       C.Name as ContainerName, C.Description as ContainerDescription, C.RoomId,
                       R.Name as RoomName, R.Description as RoomDescription
                from Items I
                join Containers C on I.ContainerId = C.Id
                join Rooms R on C.RoomId = R.Id
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP VIEW View_ItemFullView");
        }
    }
}
