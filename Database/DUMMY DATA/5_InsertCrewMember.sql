WITH RowGeneratorCTE AS (
    SELECT
        ROW_NUMBER() OVER (ORDER BY a.object_id) AS RowNum,
        CASE (ROW_NUMBER() OVER (ORDER BY a.object_id) % 4) 
            WHEN 0 THEN 'Greek' 
            WHEN 1 THEN 'Filipino' 
            WHEN 2 THEN 'Indian' 
            ELSE 'Mexican' 
        END AS Nationality
    FROM 
        sys.objects a, 
        sys.objects b
    WHERE a.object_id < 800 AND b.object_id < 800 
)
INSERT INTO CrewMember (CrewMemberId, FirstName, LastName, BirthDate, Nationality, CreatedBy)
SELECT 
    'CRW' + FORMAT(RowNum, '000'), -- CrewMemberId
    'CrewFN' + FORMAT(RowNum, '000'), -- FirstName
    'CrewLN' + FORMAT(RowNum, '000'), -- LastName
    DATEADD(YEAR, - (25 + (RowNum % 30)), GETDATE()), 
    Nationality,
    'SYSTEM'
FROM 
    RowGeneratorCTE
WHERE 
    RowNum <= 60;