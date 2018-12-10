IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[SubDealers]') 
    AND xtype IN (N'FN', N'IF', N'TF')
)
    DROP FUNCTION [dbo].[SubDealers]
GO

	CREATE FUNCTION [dbo].[SubDealers]
(@Id int)
RETURNS VARCHAR(500)
AS
BEGIN

DECLARE @temp varchar(500)

SELECT	@temp=COALESCE(@temp + ', ', '') + CAST(D.Organization AS VARCHAR(100))
FROM	TC_SubDealers SD
		INNER JOIN	Dealers D WITH(NOLOCK) ON 
					D.ID=SD.SubDealerId
WHERE	TC_DealerMappingId=@Id 
		AND IsActive=1

RETURN	(@temp)
END
