IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetCarMakeDesc]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetCarMakeDesc]
GO

	-- =============================================
-- Author:		Shalini Nair
-- Create date: 11/07/14
-- Description: Gets the car make description based on MakeId passed 
-- =============================================
CREATE PROCEDURE [dbo].[GetCarMakeDesc] @MakeId INT
AS
BEGIN
	SELECT *
	FROM MakeDescriptions WITH (NOLOCK)
	WHERE MakeId = @MakeId
END


