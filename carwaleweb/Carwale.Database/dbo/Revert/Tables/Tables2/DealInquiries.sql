
IF COL_LENGTH('dbo.DealInquiries','Eagerness') IS NOT NULL
BEGIN
    Alter table DealInquiries
    DROP COLUMN Eagerness 
END
