SET IDENTITY_INSERT [dbo].[Rank] ON;

GO

INSERT INTO [dbo].[Rank]
    ([RankCode], [RankName], [IsActive], [CreatedBy])
VALUES
    (1, 'Master', 1, 'SYSTEM'),
    (2, 'Chief Officer', 1, 'SYSTEM'),
    (3, 'Second Officer', 1, 'SYSTEM'),
    (4, 'Third Officer', 1, 'SYSTEM'),
    (5, 'Bosun', 1, 'SYSTEM'),
    (6, 'Able Seaman (AB)', 1, 'SYSTEM'),
    (7, 'Ordinary Seaman (OS)', 1, 'SYSTEM'),
    
    (8, 'Chief Engineer', 1, 'SYSTEM'),
    (9, 'Second Engineer', 1, 'SYSTEM'),
    (10, 'Third Engineer', 1, 'SYSTEM'),
    (11, 'Fourth Engineer', 1, 'SYSTEM'),
    (12, 'Electrician', 1, 'SYSTEM'),
    (13, 'Fitter', 1, 'SYSTEM'),
    (14, 'Oiler', 1, 'SYSTEM'),
    (15, 'Wiper', 1, 'SYSTEM'),
    
    (16, 'Chief Cook', 1, 'SYSTEM'),
    (17, 'Messman', 1, 'SYSTEM'),
    (18, 'Steward', 1, 'SYSTEM'),
    
    (19, 'Deck Cadet', 1, 'SYSTEM'),
    (20, 'Engine Cadet', 1, 'SYSTEM');

SET IDENTITY_INSERT [dbo].[Rank] OFF;