INSERT INTO SystemUser (UserName, UserRole,PasswordHash, CreatedBy)
VALUES
('AdminUser', 'Administrator',HASHBYTES('SHA2_256', N'123456789'),  'SYSTEM'),
('FinanceMgr', 'Finance', HASHBYTES('SHA2_256', N'123456789'),  'SYSTEM'),
('CrewPlanner', 'Operations', HASHBYTES('SHA2_256', N'123456789'),  'SYSTEM'),
('GeneralUser', 'Operations', HASHBYTES('SHA2_256', N'123456789'),  'SYSTEM');