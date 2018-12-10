IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_TransferDealer]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_TransferDealer]
GO

	
CREATE PROCEDURE [dbo].[CRM_TransferDealer]  
 @FromDC     VARCHAR(8000),  
 @ToDC       NUMERIC,  
 @LeadIds    VARCHAR(8000), 
 @TransferBy NUMERIC, 
 @DealerIds  VARCHAR(8000),
 @CallIds    VARCHAR(8000),
 @Type       SMALLINT,
 @Status     INT OUTPUT   
AS  
  
BEGIN  
	DECLARE @sDelimiter CHAR = ','
	
	UPDATE CRM_CallActiveList SET CallerId = @ToDC WHERE IsTeam = 0 AND CallId IN (SELECT * FROM dbo.list_to_tbl(@CallIds))
	
	UPDATE CRM_Calls SET CallerId = @ToDC WHERE IsTeam = 0 AND IsActionTaken = 0 AND Id IN (SELECT * FROM dbo.list_to_tbl(@CallIds))
	
	UPDATE CRM_ADM_DCDealers SET DCID =@ToDC, UpdatedBy=@TransferBy, UpdatedOn=GETDATE() WHERE DealerId IN (SELECT * FROM dbo.list_to_tbl(@DealerIds))
	
	UPDATE CRM_LeadCarOwners SET DealerCoordinator = @ToDC, UpdatedBy = @TransferBy, UpdatedOn=GETDATE() WHERE LeadId IN (SELECT * FROM dbo.list_to_tbl(@LeadIds)) AND  DealerCoordinator IN(SELECT * FROM dbo.list_to_tbl(@FromDC))
	
	-- Type 1 : Transfer Dealer, Type 2 : Transfer DC Calls
	IF (@Type = 1)
		
		BEGIN
			SET @DealerIds = @DealerIds + ','
						
		    DECLARE @FDC VARCHAR(20)
		    DECLARE @DId VARCHAR(20)
			
			WHILE CHARINDEX(@sDelimiter,@FromDC,0) <> 0
			BEGIN 
				 SELECT
				  @FDC=RTRIM(LTRIM(SUBSTRING(@FromDC,1,CHARINDEX(@sDelimiter,@FromDC,0)-1))),
				  @FromDC=RTRIM(LTRIM(SUBSTRING(@FromDC,CHARINDEX(@sDelimiter,@FromDC,0)+LEN(@sDelimiter),LEN(@FromDC)))),
				 
				  @DId=RTRIM(LTRIM(SUBSTRING(@DealerIds,1,CHARINDEX(@sDelimiter,@DealerIds,0)-1))),
				  @DealerIds=RTRIM(LTRIM(SUBSTRING(@DealerIds,CHARINDEX(@sDelimiter,@DealerIds,0)+LEN(@sDelimiter),LEN(@DealerIds))))
				  
				  IF LEN(@DId) > 0
					BEGIN
						INSERT INTO CRM_TransferDealerLog(DealerId,FromDC,ToDC,TransferBy,TransferDate)
						VALUES(@DId,@FDC, @ToDC, @TransferBy, GETDATE())
						
						SET @Status=1
					END	
			END	
		END
	ELSE
		IF (@Type = 2)
			BEGIN
			
				DECLARE @CID VARCHAR(20)
				
				WHILE CHARINDEX(@sDelimiter,@CallIds,0) <> 0
				BEGIN 
					 SELECT
					  @CID=RTRIM(LTRIM(SUBSTRING(@CallIds,1,CHARINDEX(@sDelimiter,@CallIds,0)-1))),
					  @CallIds=RTRIM(LTRIM(SUBSTRING(@CallIds,CHARINDEX(@sDelimiter,@CallIds,0)+LEN(@sDelimiter),LEN(@CallIds))))
					 
					  IF LEN(@CID) > 0
						BEGIN
							INSERT INTO CRM_TransferDCCallLog(CallId,FromDC,ToDC,CallerType,TransferBy,TransferDate)
							VALUES(@CID,@FromDC, @ToDC,1, @TransferBy, GETDATE())
							
							SET @Status=1
						END	
				END	
			END
		
END
