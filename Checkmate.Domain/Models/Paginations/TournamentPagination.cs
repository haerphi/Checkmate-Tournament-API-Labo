using Checkmate.Domain.Enums;

namespace Checkmate.Domain.Models.Paginations
{
	public class TournamentPagination : Pagination
	{
		public string? Name { get; set; } = null;
		public TournamentStatusEnum? Status { get; set; } = null;
		public string? Address { get; set; } = null;
		public string[] Categories { get; set; } = [];

		public TournamentPagination(int offset = 0, int limit = 10, string? name = null, TournamentStatusEnum? status = null, string? address = null, string[] categories = null) : base(offset, limit)
		{
			Name = name;
			Status = status;
			Address = address;
			Categories = categories;
			if (Categories is null)
			{
				Categories = [];
			}
		}

		public TournamentPagination() { }
	}
}
