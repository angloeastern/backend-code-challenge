USE [AE_Challange]
GO

/****** Object:  Table [dbo].[Ship]    Script Date: 12/15/2025 5:25:30 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Ship](
	[ShipCode] [nvarchar](10) NOT NULL,
	[ShipName] [nvarchar](100) NOT NULL,
	[FiscalYear] [int] NOT NULL,
	[Status] [nvarchar](10) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedDate] [datetime2](3) NOT NULL,
	[UpdatedDate] [datetime2](3) NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[UpdatedBy] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[ShipCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Ship] ADD  DEFAULT ((1)) FOR [IsActive]
GO

ALTER TABLE [dbo].[Ship] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO

ALTER TABLE [dbo].[Ship]  WITH CHECK ADD  CONSTRAINT [CHK_Ship_FiscalYear] CHECK  (([FiscalYear]>=(1900) AND [FiscalYear]<=(3000)))
GO

ALTER TABLE [dbo].[Ship] CHECK CONSTRAINT [CHK_Ship_FiscalYear]
GO

ALTER TABLE [dbo].[Ship]  WITH CHECK ADD  CONSTRAINT [CHK_Ship_Status] CHECK  (([Status]='Inactive' OR [Status]='Active'))
GO

ALTER TABLE [dbo].[Ship] CHECK CONSTRAINT [CHK_Ship_Status]
GO

