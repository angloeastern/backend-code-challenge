USE [AE_Challange]
GO

/****** Object:  Table [dbo].[UserShipAssignment]    Script Date: 12/15/2025 5:25:56 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[UserShipAssignment](
	[UserId] [int] NOT NULL,
	[ShipCode] [nvarchar](10) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedDate] [datetime2](3) NOT NULL,
	[UpdatedDate] [datetime2](3) NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[UpdatedBy] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[ShipCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[UserShipAssignment] ADD  DEFAULT ((1)) FOR [IsActive]
GO

ALTER TABLE [dbo].[UserShipAssignment] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO

ALTER TABLE [dbo].[UserShipAssignment]  WITH CHECK ADD  CONSTRAINT [FK_UserShipAssignment_Ship] FOREIGN KEY([ShipCode])
REFERENCES [dbo].[Ship] ([ShipCode])
GO

ALTER TABLE [dbo].[UserShipAssignment] CHECK CONSTRAINT [FK_UserShipAssignment_Ship]
GO

ALTER TABLE [dbo].[UserShipAssignment]  WITH CHECK ADD  CONSTRAINT [FK_UserShipAssignment_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[SystemUser] ([UserId])
GO

ALTER TABLE [dbo].[UserShipAssignment] CHECK CONSTRAINT [FK_UserShipAssignment_User]
GO

