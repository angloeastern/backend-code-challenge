

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AEBackend.Repositories.RepositoryUsingEF;
public class PortEntityTypeConfiguration : IEntityTypeConfiguration<Port>
{
  public void Configure(EntityTypeBuilder<Port> builder)
  {

  }
}

