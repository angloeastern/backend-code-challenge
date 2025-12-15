SET IDENTITY_INSERT [dbo].[ChartOfAccount] ON;

INSERT INTO [dbo].[ChartOfAccount]
    ([COA_ID], [AccountNumber], [AccountDescription], [ParentCOA_ID], [AccountType], [IsActive], [CreatedBy])
VALUES
    (1000, '1000', 'PENDAPATAN USAHA (REVENUE)', NULL, 'Parent', 1, 'SYSTEM'),
    (2000, '2000', 'BIAYA OPERASIONAL (OPEX)', NULL, 'Parent', 1, 'SYSTEM'),
    (3000, '3000', 'BIAYA KAPITAL (CAPEX)', NULL, 'Parent', 1, 'SYSTEM'),
    (4000, '4000', 'BIAYA GAJI & KRU (CREW EXP)', NULL, 'Parent', 1, 'SYSTEM'),
    (5000, '5000', 'PENYUSUTAN (DEPRECIATION)', NULL, 'Parent', 1, 'SYSTEM'),
    (6000, '6000', 'ADMIN & GENERAL EXPENSE', NULL, 'Parent', 1, 'SYSTEM'),

    (1010, '1010', 'Freight Revenue - Time Charter', 1000, 'Child', 1, 'SYSTEM'),
    (1020, '1020', 'Freight Revenue - Voyage Charter', 1000, 'Child', 1, 'SYSTEM'),
    (1030, '1030', 'Lain-lain Revenue', 1000, 'Child', 1, 'SYSTEM'),

    (2100, '2100', 'Fuel Oil & Lubricants', 2000, 'Parent', 1, 'SYSTEM'),
    (2200, '2200', 'Provisioning & Catering', 2000, 'Child', 1, 'SYSTEM'),
    (2300, '2300', 'Port Charges & Agency Fees', 2000, 'Child', 1, 'SYSTEM'),
    (2400, '2400', 'Repair & Maintenance', 2000, 'Parent', 1, 'SYSTEM'),
    (2500, '2500', 'Spare Parts & Stores', 2000, 'Child', 1, 'SYSTEM'),
    (2600, '2600', 'Insurance & P&I', 2000, 'Child', 1, 'SYSTEM'),
    (2700, '2700', 'Communication Expenses', 2000, 'Child', 1, 'SYSTEM'),

    (2110, '2110', 'Heavy Fuel Oil (HFO)', 2100, 'Child', 1, 'SYSTEM'),
    (2120, '2120', 'Marine Diesel Oil (MDO)', 2100, 'Child', 1, 'SYSTEM'),
    (2130, '2130', 'Lubricating Oil', 2100, 'Child', 1, 'SYSTEM'),

    (2410, '2410', 'Engine Repair', 2400, 'Child', 1, 'SYSTEM'),
    (2420, '2420', 'Hull & Deck Repair', 2400, 'Child', 1, 'SYSTEM'),
    (2430, '2430', 'Safety Equipment Maintenance', 2400, 'Child', 1, 'SYSTEM'),

    (3100, '3100', 'Drydocking Expenses', 3000, 'Child', 1, 'SYSTEM'),
    (3200, '3200', 'New Equipment Purchase (Capitalized)', 3000, 'Child', 1, 'SYSTEM'),

    (4100, '4100', 'Wages & Salary', 4000, 'Child', 1, 'SYSTEM'),
    (4200, '4200', 'Repatriation & Travel Cost', 4000, 'Child', 1, 'SYSTEM'),
    (4300, '4300', 'Training & Certification', 4000, 'Child', 1, 'SYSTEM'),

    (5100, '5100', 'Ship Depreciation', 5000, 'Child', 1, 'SYSTEM'),

    (6100, '6100', 'Office Overhead', 6000, 'Child', 1, 'SYSTEM'),
    (6200, '6200', 'IT & Software License', 6000, 'Child', 1, 'SYSTEM');

GO

SET IDENTITY_INSERT [dbo].[ChartOfAccount] OFF;