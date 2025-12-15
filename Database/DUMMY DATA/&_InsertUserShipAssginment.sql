INSERT INTO UserShipAssignment (UserId, ShipCode, CreatedBy, IsActive)
VALUES
((SELECT UserId FROM SystemUser WHERE UserName = 'FinanceMgr'), 'SHIP001', 'SYSTEM', 1),
((SELECT UserId FROM SystemUser WHERE UserName = 'FinanceMgr'), 'SHIP002', 'SYSTEM', 1),
((SELECT UserId FROM SystemUser WHERE UserName = 'CrewPlanner'), 'SHIP001', 'SYSTEM', 1),
((SELECT UserId FROM SystemUser WHERE UserName = 'CrewPlanner'), 'SHIP005', 'SYSTEM', 1); 