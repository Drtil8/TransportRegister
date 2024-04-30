CREATE PROCEDURE ExecuteBadPointsUpdate
    AS
BEGIN
    -- Odečtení bodů za trestné činy, které jsou starší než 1 rok od LastCrimeCommited
UPDATE Persons
SET BadPoints = CASE
                    WHEN DATEDIFF(YEAR, LastCrimeCommited, GETDATE()) >= 1 THEN
                        CASE
                            WHEN BadPoints >= 4 THEN BadPoints - 4
                            ELSE 0
                            END
                    ELSE BadPoints
    END
END