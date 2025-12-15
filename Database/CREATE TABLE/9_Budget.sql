USE [AE_Challange]
GO

/****** Object:  Table [dbo].[Budget]    Script Date: 12/15/2025 5:27:33 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Budget](
	[BudgetID] [bigint] IDENTITY(1,1) NOT NULL,
	[ShipCode] [nvarchar](10) NOT NULL,
	[COA_ID] [int] NOT NULL,
	[PeriodID] [int] NOT NULL,
	[BudgetTarget] [decimal](19, 2) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedDate] [datetime2](3) NOT NULL,
	[UpdatedDate] [datetime2](3) NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[UpdatedBy] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[BudgetID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_Budget] UNIQUE NONCLUSTERED 
(
	[ShipCode] ASC,
	[COA_ID] ASC,
	[PeriodID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Budget] ADD  DEFAULT ((0)) FOR [BudgetTarget]
GO

ALTER TABLE [dbo].[Budget] ADD  DEFAULT ((1)) FOR [IsActive]
GO

ALTER TABLE [dbo].[Budget] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO

ALTER TABLE [dbo].[Budget]  WITH CHECK ADD  CONSTRAINT [FK_Budget_COA] FOREIGN KEY([COA_ID])
REFERENCES [dbo].[ChartOfAccount] ([COA_ID])
GO

ALTER TABLE [dbo].[Budget] CHECK CONSTRAINT [FK_Budget_COA]
GO

ALTER TABLE [dbo].[Budget]  WITH CHECK ADD  CONSTRAINT [FK_Budget_Period] FOREIGN KEY([PeriodID])
REFERENCES [dbo].[FiscalPeriod] ([PeriodID])
GO

ALTER TABLE [dbo].[Budget] CHECK CONSTRAINT [FK_Budget_Period]
GO

ALTER TABLE [dbo].[Budget]  WITH CHECK ADD  CONSTRAINT [FK_Budget_Ship] FOREIGN KEY([ShipCode])
REFERENCES [dbo].[Ship] ([ShipCode])
GO

ALTER TABLE [dbo].[Budget] CHECK CONSTRAINT [FK_Budget_Ship]
GO

