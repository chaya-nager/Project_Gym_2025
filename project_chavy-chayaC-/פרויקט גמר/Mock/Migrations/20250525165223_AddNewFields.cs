using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mock.Migrations
{
    /// <inheritdoc />
    public partial class AddNewFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserWorkoutPlans_Users_UserId",
                table: "UserWorkoutPlans");

            migrationBuilder.DropForeignKey(
                name: "FK_UserWorkoutPlans_WorkoutVideos_VideoId",
                table: "UserWorkoutPlans");

            migrationBuilder.DropIndex(
                name: "IX_UserWorkoutPlans_UserId",
                table: "UserWorkoutPlans");

            migrationBuilder.DropIndex(
                name: "IX_UserWorkoutPlans_VideoId",
                table: "UserWorkoutPlans");

            migrationBuilder.DropColumn(
                name: "VideoId",
                table: "UserWorkoutPlans");

            migrationBuilder.AddColumn<int>(
                name: "UserWorkoutPlanId",
                table: "WorkoutVideos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkoutVideo",
                table: "WorkoutVideos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutVideos_UserWorkoutPlanId",
                table: "WorkoutVideos",
                column: "UserWorkoutPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_UserWorkoutPlans_UserId",
                table: "UserWorkoutPlans",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UserWorkoutPlans_Users_UserId",
                table: "UserWorkoutPlans",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkoutVideos_UserWorkoutPlans_UserWorkoutPlanId",
                table: "WorkoutVideos",
                column: "UserWorkoutPlanId",
                principalTable: "UserWorkoutPlans",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserWorkoutPlans_Users_UserId",
                table: "UserWorkoutPlans");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutVideos_UserWorkoutPlans_UserWorkoutPlanId",
                table: "WorkoutVideos");

            migrationBuilder.DropIndex(
                name: "IX_WorkoutVideos_UserWorkoutPlanId",
                table: "WorkoutVideos");

            migrationBuilder.DropIndex(
                name: "IX_UserWorkoutPlans_UserId",
                table: "UserWorkoutPlans");

            migrationBuilder.DropColumn(
                name: "UserWorkoutPlanId",
                table: "WorkoutVideos");

            migrationBuilder.DropColumn(
                name: "WorkoutVideo",
                table: "WorkoutVideos");

            migrationBuilder.AddColumn<int>(
                name: "VideoId",
                table: "UserWorkoutPlans",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UserWorkoutPlans_UserId",
                table: "UserWorkoutPlans",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserWorkoutPlans_VideoId",
                table: "UserWorkoutPlans",
                column: "VideoId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserWorkoutPlans_Users_UserId",
                table: "UserWorkoutPlans",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserWorkoutPlans_WorkoutVideos_VideoId",
                table: "UserWorkoutPlans",
                column: "VideoId",
                principalTable: "WorkoutVideos",
                principalColumn: "VideoId");
        }
    }
}
