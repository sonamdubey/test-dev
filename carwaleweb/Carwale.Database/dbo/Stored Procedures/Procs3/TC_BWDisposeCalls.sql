IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_BWDisposeCalls]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_BWDisposeCalls]
GO

	
-- =============================================
-- Author:		Kritika Choudhary
-- Create date: <Create Date, 18th Jan, 2016>
-- Description:	Finds disposed calls
-- Sample Input : '1,2,3'
-- EXEC TC_BWDisposeCalls '1,2,3'
-- =============================================
CREATE PROCEDURE [dbo].[TC_BWDisposeCalls] 
  @CallIds    VARCHAR(MAX)
  
AS  
  
BEGIN  

	DECLARE @sDelimiter CHAR= ',', @CID VARCHAR(20),@DisposedCalls VARCHAR(MAX) = NULL
	
	SET @CallIds = @CallIds + ','
	DECLARE @TempCallIds Table (callId INT)
	
	WHILE CHARINDEX(@sDelimiter,@CallIds,0) <> 0
	BEGIN 
	
		 SET @CID=RTRIM(LTRIM(SUBSTRING(@CallIds,1,CHARINDEX(@sDelimiter,@CallIds,0)-1)))
		  SET @CallIds=RTRIM(LTRIM(SUBSTRING(@CallIds,CHARINDEX(@sDelimiter,@CallIds,0)+LEN(@sDelimiter),LEN(@CallIds))))
		 
		  IF LEN(@CID) > 0
			BEGIN
		     	IF NOT EXISTS(SELECT TC_CallsId FROM TC_ActiveCalls WITH(NOLOCK) WHERE TC_CallsId=@CID)
				BEGIN
				INSERT INTO @TempCallIds (callId) VALUES(@CID)
						--SELECT @CID
					--SET @DisposedCalls=ISNULL(@DisposedCalls,'') + @CID + ','
				END
			END	
	END	
	--SET @DisposedCalls=LEFT(@DisposedCalls, LEN(@DisposedCalls) - 1)
	--SELECT @DisposedCalls AS DispCallIds
	SELECT * FROM @TempCallIds
END

