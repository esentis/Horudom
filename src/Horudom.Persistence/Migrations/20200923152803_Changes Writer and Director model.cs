//<auto-generated />

using System;

using Microsoft.EntityFrameworkCore.Migrations;

namespace Esentis.Horudom.Persistence.Migrations
{
	public partial class ChangesWriterandDirectormodel : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropColumn(
				name: "Firstname",
				table: "Writers");

			migrationBuilder.DropColumn(
				name: "Lastname",
				table: "Writers");

			migrationBuilder.DropColumn(
				name: "NormalizedFirstname",
				table: "Writers");

			migrationBuilder.DropColumn(
				name: "NormalizedLastname",
				table: "Writers");

			migrationBuilder.DropColumn(
				name: "Firstname",
				table: "Directors");

			migrationBuilder.DropColumn(
				name: "Lastname",
				table: "Directors");

			migrationBuilder.DropColumn(
				name: "NormalizedFirstname",
				table: "Directors");

			migrationBuilder.DropColumn(
				name: "NormalizedLastname",
				table: "Directors");

			migrationBuilder.AddColumn<DateTimeOffset>(
				name: "LastScrape",
				table: "Writers",
				nullable: false,
				defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

			migrationBuilder.AddColumn<string>(
				name: "Name",
				table: "Writers",
				nullable: false,
				defaultValue: "");

			migrationBuilder.AddColumn<string>(
				name: "NormalizedName",
				table: "Writers",
				nullable: false,
				defaultValue: "");

			migrationBuilder.AddColumn<long>(
				name: "TmdbId",
				table: "Writers",
				nullable: true);

			migrationBuilder.AddColumn<DateTimeOffset>(
				name: "LastScrape",
				table: "Directors",
				nullable: false,
				defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

			migrationBuilder.AddColumn<string>(
				name: "Name",
				table: "Directors",
				nullable: false,
				defaultValue: "");

			migrationBuilder.AddColumn<string>(
				name: "NormalizedName",
				table: "Directors",
				nullable: false,
				defaultValue: "");

			migrationBuilder.AddColumn<long>(
				name: "TmdbId",
				table: "Directors",
				nullable: true);
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropColumn(
				name: "LastScrape",
				table: "Writers");

			migrationBuilder.DropColumn(
				name: "Name",
				table: "Writers");

			migrationBuilder.DropColumn(
				name: "NormalizedName",
				table: "Writers");

			migrationBuilder.DropColumn(
				name: "TmdbId",
				table: "Writers");

			migrationBuilder.DropColumn(
				name: "LastScrape",
				table: "Directors");

			migrationBuilder.DropColumn(
				name: "Name",
				table: "Directors");

			migrationBuilder.DropColumn(
				name: "NormalizedName",
				table: "Directors");

			migrationBuilder.DropColumn(
				name: "TmdbId",
				table: "Directors");

			migrationBuilder.AddColumn<string>(
				name: "Firstname",
				table: "Writers",
				type: "text",
				nullable: false,
				defaultValue: "");

			migrationBuilder.AddColumn<string>(
				name: "Lastname",
				table: "Writers",
				type: "text",
				nullable: false,
				defaultValue: "");

			migrationBuilder.AddColumn<string>(
				name: "NormalizedFirstname",
				table: "Writers",
				type: "text",
				nullable: false,
				defaultValue: "");

			migrationBuilder.AddColumn<string>(
				name: "NormalizedLastname",
				table: "Writers",
				type: "text",
				nullable: false,
				defaultValue: "");

			migrationBuilder.AddColumn<string>(
				name: "Firstname",
				table: "Directors",
				type: "text",
				nullable: false,
				defaultValue: "");

			migrationBuilder.AddColumn<string>(
				name: "Lastname",
				table: "Directors",
				type: "text",
				nullable: false,
				defaultValue: "");

			migrationBuilder.AddColumn<string>(
				name: "NormalizedFirstname",
				table: "Directors",
				type: "text",
				nullable: false,
				defaultValue: "");

			migrationBuilder.AddColumn<string>(
				name: "NormalizedLastname",
				table: "Directors",
				type: "text",
				nullable: false,
				defaultValue: "");
		}
	}
}
