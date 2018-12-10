IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_TransferPECall]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_TransferPECall]
GO

	
CREATE PROCEDURE [dbo].[CRM_TransferPECall]  
 @FromCC     NUMERIC,  
 @ToCC       NUMERIC,  
 @LeadIds    VARCHAR(8000), 
 @CallIds    VARCHAR(8000),
 @TransferBy NUMERIC, 
 @Status     INT OUTPUT   
AS  
  
BEGIN  
	DECLARE @sDelimiter CHAR= ','
	
	UPDATE CRM_CallActiveList SET CallerId = @ToCC WHERE IsTeam = 0 AND CallId IN (SELECT * FROM dbo.list_to_tbl(@CallIds))
	
	UPDATE CRM_Calls SET CallerId = @ToCC WHERE IsTeam = 0 AND IsActionTaken = 0 AND Id IN (SELECT * FROM dbo.list_to_tbl(@CallIds))
	
	UPDATE CRM_LeadCarOwners SET CarConsultant = @ToCC, UpdatedBy = @TransferBy, UpdatedOn=GETDATE() WHERE LeadId IN (SELECT * FROM dbo.list_to_tbl(@LeadIds))
	
	DECLARE @CID VARCHAR(20)
	
	WHILE CHARINDEX(@sDelimiter,@CallIds,0) <> 0
	BEGIN 
		 SELECT
		  @CID=RTRIM(LTRIM(SUBSTRING(@CallIds,1,CHARINDEX(@sDelimiter,@CallIds,0)-1))),
		  @CallIds=RTRIM(LTRIM(SUBSTRING(@CallIds,CHARINDEX(@sDelimiter,@CallIds,0)+LEN(@sDelimiter),LEN(@CallIds))))
		 
		  IF LEN(@CID) > 0
			BEGIN
				INSERT INTO CRM_TransferDCCallLog(CallId,FromDC,ToDC,CallerType,TransferBy,TransferDate)
				VALUES(@CID,@FromCC, @ToCC,2, @TransferBy, GETDATE())
				
				SET @Status=1
			END	
	END	
END
