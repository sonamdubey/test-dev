IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BW_AddBikeMakes]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BW_AddBikeMakes]
GO

	
-- =============================================
-- Author:		vivek gupta
-- Create date: 11-07-2016
-- Description:	Add bike makes
-- =============================================
create PROCEDURE BW_AddBikeMakes
  @MakeName VARCHAR(30)
, @MaskingName VARCHAR(30)
, @New BIT = NULL
, @Used BIT = NULL
, @Futuristic BIT = NULL
, @MakeId INT = 0 
AS
BEGIN	
	SET NOCOUNT ON;

	INSERT INTO BikeMakes(
	     Id
		,NAME			
		,MaskingName
		,New
		,Used
		,Futuristic
		,IsDeleted
		)
	VALUES (@MakeId, @MakeName, @MaskingName, ISNULL(@New,1), ISNULL(@Used,1), ISNULL(@Futuristic,0), 0)
	-- SET @Id = SCOPE_IDENTITY()
    
END
