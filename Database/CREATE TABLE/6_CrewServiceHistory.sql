USE [AE_Challange]
GO

/****** Object:  Table [dbo].[CrewServiceHistory]    Script Date: 12/15/2025 5:26:24 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CrewServiceHistory](
	[ServiceHistoryId] [bigint] IDENTITY(1,1) NOT NULL,
	[CrewMemberId] [nvarchar](10) NOT NULL,
	[ShipCode] [nvarchar](10) NOT NULL,
	[RankCode] [int] NOT NULL,
	[SignOnDate] [date] NOT NULL,
	[SignOffDate] [date] NULL,
	[EndOfContractDate] [date] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedDate] [datetime2](3) NOT NULL,
	[UpdatedDate] [datetime2](3) NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[UpdatedBy] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[ServiceHistoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[CrewServiceHistory] ADD  DEFAULT ((1)) FOR [IsActive]
GO

ALTER TABLE [dbo].[CrewServiceHistory] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO

ALTER TABLE [dbo].[CrewServiceHistory]  WITH CHECK ADD  CONSTRAINT [FK_CSH_CrewMember] FOREIGN KEY([CrewMemberId])
REFERENCES [dbo].[CrewMember] ([CrewMemberId])
GO

ALTER TABLE [dbo].[CrewServiceHistory] CHECK CONSTRAINT [FK_CSH_CrewMember]
GO

ALTER TABLE [dbo].[CrewServiceHistory]  WITH CHECK ADD  CONSTRAINT [FK_CSH_Rank] FOREIGN KEY([RankCode])
REFERENCES [dbo].[Rank] ([RankCode])
GO

ALTER TABLE [dbo].[CrewServiceHistory] CHECK CONSTRAINT [FK_CSH_Rank]
GO

ALTER TABLE [dbo].[CrewServiceHistory]  WITH CHECK ADD  CONSTRAINT [FK_CSH_Ship] FOREIGN KEY([ShipCode])
REFERENCES [dbo].[Ship] ([ShipCode])
GO

ALTER TABLE [dbo].[CrewServiceHistory] CHECK CONSTRAINT [FK_CSH_Ship]
GO

ALTER TABLE [dbo].[CrewServiceHistory]  WITH CHECK ADD  CONSTRAINT [CHK_ServiceDates] CHECK  (([SignOnDate]<[EndOfContractDate] OR [SignOffDate] IS NOT NULL))
GO

ALTER TABLE [dbo].[CrewServiceHistory] CHECK CONSTRAINT [CHK_ServiceDates]
GO

