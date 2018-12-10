IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[OLM_AddSkodaDealers]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[OLM_AddSkodaDealers]
GO

	

-- =============================================
-- Author:		Vaibhav Kale
-- Create date: 14-02-2012
-- Description:	Adding a New Skoda Dealer
-- =============================================
CREATE PROCEDURE [dbo].[OLM_AddSkodaDealers] 
	-- Add the parameters for the stored procedure here
	@Check			BIT,
	@DealerName		VARCHAR(100)=NULL,
	@DealerCode		VARCHAR(50)=NULL,
	@DealerId		NUMERIC(18),
	@OutletName		VARCHAR(50),
	@OutletCode		VARCHAR(50),
	@CityId			NUMERIC(18),
	
	@DealerP		VARCHAR(500)=NULL,
	@DealerPMob		VARCHAR(500)=NULL,
	@DealerPEmail	VARCHAR(500)=NULL,
	@ContactP		VARCHAR(500)=NULL,
	@ContactPMob	VARCHAR(500)=NULL,
	@ContactPEmail	VARCHAR(500)=NULL,
	@ShowAddr		VARCHAR(500)=NULL,
	@ShowNum		VARCHAR(500)=NULL,
	@ShowEmail		VARCHAR(500)=NULL, 
	@ShowFax		VARCHAR(500)=NULL,
	@ServiceC1Addr	VARCHAR(500)=NULL,
	@ServiceC2Addr	VARCHAR(500)=NULL,
	@ServiceOutletName VARCHAR(50)=NULL,
	@ServiceOutlet2Name VARCHAR(50)=NULL,
	@ServiceCNum	VARCHAR(500)=NULL,
	@ServiceCEmail	VARCHAR(500)=NULL,
	@ServiceCFax	VARCHAR(500)=NULL,
	@ServiceCenter2ContactNo	VARCHAR(500)=NULL,
	@ServiceCenter2Email		VARCHAR(500)=NULL,
	@IsShow			BIT,
	@IsService		BIT,
	@IsRapid		BIT=0,
	@ShowLat		VARCHAR(50)=NULL,
	@ShowLong		VARCHAR(50)=NULL,
	@Service1Lat	VARCHAR(50)=NULL,
	@Service1Long	VARCHAR(50)=NULL,
	@Service2Lat	VARCHAR(50)=NULL,
	@Service2Long	VARCHAR(50)=NULL,
	@UpdatedBy		NUMERIC(18),
	@UpdatedOn		DATETIME=GETDATE,
	@Message		VARCHAR(100)='SOME ERROR' OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON
		DECLARE @SkodaDealerId INT,
				@NumberRecords INT = 0
	BEGIN TRY
		IF(@Check = 0)--If TextBox is selected and new dealer is created
			BEGIN
				SELECT Id FROM FB_SkodaDealers WITH (NOLOCK) WHERE DealerCode = @DealerCode
				SET @NumberRecords = @@ROWCOUNT
				
				IF(@NumberRecords = 0)--If no record exixt for given DealerCode
					BEGIN
						INSERT INTO FB_SkodaDealers (DealerName,DealerCode)
						VALUES (@DealerName,@DealerCode)
						SET @SkodaDealerId = SCOPE_IDENTITY()
					END
					
				ELSE
					BEGIN
						SET @Message = 'INSERT FAILED: DEALER CODE ALREADY EXIST'
					END
			END
		
		ELSE
			BEGIN--Takes the DealerId that is passed as a parameter
				SET @SkodaDealerId = @DealerId
			END
	END TRY
	
	BEGIN CATCH
		SET @Message = 'SOME ERROR OCCURED'
	END CATCH
	
	IF(@NumberRecords = 0)
	BEGIN
		INSERT INTO FB_SkodaDealerDetails
					(SkodaDealerId,CityId,DealerPrinciple,DMobileNoAndEmail,ContactPerson,
					CMobileNoAndEmail,ShowroomAddr,ShowroomContactNo,ShowroomEmail,ShowroomFax,
					ServiceCenter1Addr,ServiceCenter2Addr,ServiceCenterContactNo,ServiceCenterFax,
					ServiceCenterEmail,IsShowroom,IsServiceCenter,OutletName,OutletCode,SLattitude,
					SLongitude,WSLattitude,WSLongitude,WS2Lattitude,WS2Longitude,IsRapidOutlet,UpdatedBy,
					UpdatedOn,ServiceOutletName,ServiceOutlet2Name,ServiceCenter2ContactNo,ServiceCenter2Email)
					VALUES(@SkodaDealerId,@CityId,@DealerP,@DealerPMob+','+@DealerPEmail,@ContactP,
					@ContactPMob+','+@ContactPEmail,@ShowAddr,@ShowNum,@ShowEmail,@ShowFax,@ServiceC1Addr,
					@ServiceC2Addr,@ServiceCNum,@ServiceCFax,@ServiceCEmail,@IsShow,@IsService,@OutletName,
					@OutletCode,@ShowLat,@ShowLong,@Service1Lat,@Service1Long,@Service2Lat,@Service2Long,@IsRapid,
					@UpdatedBy,@UpdatedOn,@ServiceOutletName,@ServiceOutlet2Name,@ServiceCenter2ContactNo,@ServiceCenter2Email)
		
		SET @Message = 'INSERT SUCCESFULL'
	END
END
