IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[SetHDFCCalculatedEMI]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[SetHDFCCalculatedEMI]
GO

	-- =============================================
-- Author:		Avishkar
-- Create date: 11-10-2012
-- Description:	Either set or remove HDFC Calculated EMI 
-- =============================================
CREATE PROCEDURE [dbo].[SetHDFCCalculatedEMI]
(@DealerId int, @IsActive bit)
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	DECLARE @TempInquiries TABLE 
	(
	    DealerId INT,
	    Inquiryid BIGINT,
	    CalculatedEMI BIGINT
	)
	
	IF(@IsActive=0)
	BEGIN	
	
	    Insert into @TempInquiries(DealerId,Inquiryid)
	    select DealerId,SI.ID
	    from Sellinquiries as SI
	    WHERE DealerId=@DealerId 
		 AND CalculatedEMI IS NOT NULL
	
		UPDATE Sellinquiries
		SET CalculatedEMI=NULL
		WHERE DealerId=@DealerId 
		 AND CalculatedEMI IS NOT NULL
		 
		UPDATE LIVELISTINGS
		set CalculatedEMI = null
		FROM LIVELISTINGS AS LL
		 JOIN @TempInquiries as Temp on LL.Inquiryid=Temp.Inquiryid
		where SellerType=1	
		 	
	END
	
	IF(@IsActive=1)
	BEGIN
	    
	    Insert into @TempInquiries(DealerId,Inquiryid,CalculatedEMI)
	    select DealerId,SI.ID,dbo.CalculateEMI(SI.Price,DATEDIFF(YEAR,SI.MakeYear,GETDATE()),16.5)
	    from Sellinquiries as SI	   
	    INNER JOIN TC_Stock TS ON TS.Id=SI.TC_StockId 
		INNER JOIN TC_CarCondition SID ON TS.Id=SID.StockId
		WHERE DealerId=@DealerId
		AND dbo.HDFCVehicleEligiblity(DealerId,SI.Price,Owners,DATEDIFF(YEAR,SI.MakeYear,GETDATE()))=1
		AND TS.IsSychronizedCW=1 AND SI.StatusId=1 
		
		 	
		UPDATE Sellinquiries
		SET CalculatedEMI=Temp.CalculatedEMI
		FROM Sellinquiries as SI
		JOIN @TempInquiries as Temp on SI.ID=Temp.Inquiryid
		
		UPDATE LIVELISTINGS
		set CalculatedEMI = SI.CalculatedEMI
		FROM LIVELISTINGS AS LL
		  JOIN Sellinquiries as SI on SI.ID=LL.Inquiryid
		  JOIN @TempInquiries as Temp on LL.Inquiryid=Temp.Inquiryid
		where SellerType=1	
		
		
	END
	
    
END
