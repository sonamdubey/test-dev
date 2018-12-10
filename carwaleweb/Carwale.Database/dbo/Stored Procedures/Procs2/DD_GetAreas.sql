IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DD_GetAreas]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DD_GetAreas]
GO

	
-------------------------------------------------------------------------------------------

-- =============================================
-- Author:		<Khushaboo Patil>
-- Create date: <2/10/2014>
-- Description:	<Get Area against pincode>
-- =============================================
CREATE PROCEDURE [dbo].[DD_GetAreas]
	@PinCode	VARCHAR(10)

AS
BEGIN
	SELECT A.ID AS Value, A.Name AS Text , A.CityId , CT.StateId
	FROM Areas A
	INNER JOIN Cities CT WITH(NOLOCK) ON CT.ID = CityId
	WHERE PinCode = @PinCode
END

