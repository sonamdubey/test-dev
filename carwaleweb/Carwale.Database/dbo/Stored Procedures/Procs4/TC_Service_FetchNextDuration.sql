IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_Service_FetchNextDuration]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_Service_FetchNextDuration]
GO

	-- =============================================
-- Author:		Nilima More 
-- Create date: 12th July 2016
-- Description:	To fetch service duration based on MakeId
--EXEC [TC_Service_FetchNextDuration] 10,1,null
-- =============================================
CREATE PROCEDURE [dbo].[TC_Service_FetchNextDuration]
	@MakeId INT 
	,@ServiceType TINYINT
	
AS
BEGIN

		DECLARE  @Duration INT = 0
		SELECT @Duration = Duration FROM TC_Service_Duration WITH(NOLOCK)
		WHERE MakeId = @MakeId AND ServiceType = @ServiceType
		SELECT @Duration 'Duration'
	
END


