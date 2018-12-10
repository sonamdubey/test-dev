IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_MatchViewInqStockCount]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_MatchViewInqStockCount]
GO

	-- =============================================
-- Author:		Ranjeet Kumar
-- Create date: 01-Jul-2014
-- Description:	Get the count for Inq and Stock (match View) for the task Page.
-- =============================================
CREATE PROCEDURE [dbo].[TC_MatchViewInqStockCount]
	@LeadIdList VARCHAR(500),
	@BranchId INT
AS
--DECLARE @BranchId BIGINT = 5  
--DECLARE @LeadId BIGINT = 6882

   
BEGIN  
	 SET NOCOUNT OFF;  
	--DROP TABLE #TC_InquiriesLead
	DECLARE @TC_InquiriesLead TABLE 
					(TC_InquiriesLeadId BIGINT ,BranchId INT,TC_LeadId BIGINT,
					 TC_LeadInquiryTypeId INT,LatestInquiryDate DATETIME )


	DECLARE @TC_InquiriesLeadWithId TABLE 
					(tblId INT IDENTITY(1,1) PRIMARY KEY, Id BIGINT ,TC_LeadInquiryTypeId INT,StockId BIGINT,
					 StockStatus TINYINT,LeadId BIGINT, newTblId BIGINT )

	DECLARE @TC_MatchViewSum TABLE 
				(Id INT IDENTITY(1,1) PRIMARY KEY,TotalBuyerInquiry INT ,TotalSellerInq INT,TotalMatchStock INT, TotalMatchStockOld INT, CarName VARCHAR(100), LeadId INT DEFAULT NULL)


	INSERT INTO @TC_InquiriesLead 
	SELECT TC_InquiriesLeadId,
		BranchId,
		TC_LeadId,
		TC_LeadInquiryTypeId,
		LatestInquiryDate
	  	
	  FROM TC_InquiriesLead as L1 WITH (NOLOCK) 
	  INNER JOIN [dbo].[fnSplitCSV](@LeadIdList) AS LS ON LS.ListMember =  L1.TC_LeadId
	  WHERE L1.BranchId=@BranchId ;--ANd L1.TC_LeadId  [dbo].[fnSplitCSV](@LeadIdList); --AND L1.TC_UserId = @UserId;
	  
	 WITH CteLeadDetails AS 
				 (  SELECT  B.TC_BuyerInquiriesId as Id,L.TC_LeadInquiryTypeId, CONVERT(VARCHAR, S.Id) as StockId,CONVERT					(VARCHAR, S.StatusId) as StockStatus,L.TC_LeadId AS LeadId

					 FROM        @TC_InquiriesLead L 
					 INNER JOIN  TC_BuyerInquiries B WITH (NOLOCK)  ON L.TC_InquiriesLeadId=B.TC_InquiriesLeadId 
					 INNER JOIN  TC_Stock          S WITH (NOLOCK)  ON B.StockId = S.Id  
					 INNER JOIN  vwMMV             V WITH (NOLOCK)  ON V.VersionId = S.VersionId    
					 WHERE L.BranchId=@BranchId --ANd L.TC_LeadId=@LeadId 
					
					   AND (B.TC_LeadDispositionID IS NULL OR B.TC_LeadDispositionId = 4)
				 UNION ALL  ---------------
					 SELECT  B.TC_BuyerInquiriesId as Id,L.TC_LeadInquiryTypeId,null,null,L.TC_LeadId AS LeadId
					 					 FROM       @TC_InquiriesLead   L  
					 INNER JOIN TC_BuyerInquiries   B WITH (NOLOCK)  ON L.TC_InquiriesLeadId=B.TC_InquiriesLeadId   
					 WHERE L.BranchId=@BranchId 
					   AND B.StockId IS NULL   --ANd L.TC_LeadId=@LeadId 
					   AND (B.TC_LeadDispositionId IS NULL OR B.TC_LeadDispositionId = 4)
		        UNION ALL   ----------------
		            SELECT  SL.TC_SellerInquiriesId as Id,L.TC_LeadInquiryTypeId,ISNULL(CONVERT(VARCHAR,ST.Id),null),null,L.TC_LeadId AS LeadId
		                   FROM            @TC_InquiriesLead  L  
		                   INNER JOIN      TC_SellerInquiries SL WITH (NOLOCK) ON L.TC_InquiriesLeadId=SL.TC_InquiriesLeadId   
						   INNER JOIN      vwMMV              V                ON SL.CarVersionId = V.VersionId  
						   LEFT OUTER JOIN TC_Stock           ST WITH (NOLOCK) ON ST.TC_SellerInquiriesId = SL.TC_SellerInquiriesId   
		                WHERE  L.BranchId=@BranchId 
		                   --AND L.TC_LeadId=@LeadId 
		                 
		                   AND (SL.TC_LeadDispositionID IS NULL OR SL.TC_LeadDispositionID = 4)
		       UNION ALL   ------------------
		          SELECT  N.TC_NewCarInquiriesId as Id,L.TC_LeadInquiryTypeId,null,null 
				  ,L.TC_LeadId AS LeadId
		          FROM            TC_NewCarInquiries N   WITH (NOLOCK) 
		          INNER JOIN      @TC_InquiriesLead  L                 ON L.TC_InquiriesLeadId =N.TC_InquiriesLeadId  
		          INNER JOIN      vwMMV              V                 ON N.VersionId = V.VersionId
		          LEFT OUTER JOIN TC_TDCalendar      TDC WITH (NOLOCK) ON TDC.TC_TDCalendarId = N.TC_TDCalendarId  
		          LEFT OUTER JOIN TC_PqRequest       PQ  WITH (NOLOCK) ON PQ.TC_NewCarInquiriesId = N.TC_NewCarInquiriesId 
		          LEFT OUTER JOIN TC_NewCarBooking   NCB WITH(NOLOCK)  ON N.TC_NewCarInquiriesId  = NCB.TC_NewCarInquiriesId
		        WHERE L.BranchId=@BranchId  
		         -- AND L.TC_LeadId=@LeadId 
		         
		          AND (N.TC_LeadDispositionId IS NULL OR (N.TC_LeadDispositionId = 4 AND ISNULL(N.CarDeliveryStatus, 0) <> 77)))
	 

	INSERT INTO @TC_InquiriesLeadWithId  select *, isnull(stockid,id) AS newTblId from CteLeadDetails

	--use While loop to Excute Sp
	 DECLARE @WhileLoopcontrol INT =1,
			 @WhileLoopCount INT ,
			 @StockId BIGINT,
			 @TC_SellerInquiriesId BIGINT,
			 @TC_BuyInqWithoutStockId BIGINT,
			 @LeadId INT

    SELECT @WhileLoopCount=Count(*)
	 FROM @TC_InquiriesLeadWithId

	 WHILE (@WhileLoopcontrol<=@WhileLoopCount)
	 BEGIN 
	 --Stock
	 IF (SELECT LWI.StockStatus FROM @TC_InquiriesLeadWithId AS LWI WHERE LWI.tblId = @WhileLoopcontrol ) = 1
		 BEGIN
		   SELECT @StockId= LWI.newTblId  FROM  @TC_InquiriesLeadWithId AS LWI
										 WHERE LWI.TblId=@WhileLoopcontrol;
			
			--excute Sp
		INSERT INTO @TC_MatchViewSum (TotalBuyerInquiry,TotalSellerInq,TotalMatchStock,TotalMatchStockOld,CarName) 
		 EXEC  [dbo].[TC_MatchingLeadsGetCount] @BranchId,@StockId, NULL, NULL	
		END
	ELSE
	---buyer
		BEGIN
			IF (SELECT LWI.TC_LeadInquiryTypeId FROM @TC_InquiriesLeadWithId AS LWI WHERE LWI.tblId = @WhileLoopcontrol) = 1 --buyer
			 BEGIN
				 SELECT @TC_BuyInqWithoutStockId= LWI.newTblId  FROM  @TC_InquiriesLeadWithId AS LWI
											 WHERE LWI.TblId=@WhileLoopcontrol;
				
				--excute Sp
				INSERT INTO @TC_MatchViewSum (TotalBuyerInquiry,TotalSellerInq,TotalMatchStock,TotalMatchStockOld,CarName)
				EXEC  [dbo].[TC_MatchingLeadsGetCount] @BranchId, NULL,NULL, @TC_BuyInqWithoutStockId	
			END
			ELSE
			--seller
				BEGIN
					IF (SELECT LWI.TC_LeadInquiryTypeId FROM @TC_InquiriesLeadWithId AS LWI 
					WHERE LWI.tblId = @WhileLoopcontrol) = 2 --seller
					 BEGIN
						 SELECT @TC_SellerInquiriesId= LWI.newTblId  FROM  @TC_InquiriesLeadWithId AS LWI
													 WHERE LWI.TblId=@WhileLoopcontrol;
												
						--excute Sp
					INSERT INTO @TC_MatchViewSum (TotalBuyerInquiry,TotalSellerInq,TotalMatchStock,TotalMatchStockOld,CarName)
						 EXEC  [dbo].[TC_MatchingLeadsGetCount] @BranchId, NULL,@TC_SellerInquiriesId, NULL	
					END
				END

		END
--Update to add LeadId 
	UPDATE @TC_MatchViewSum SET LeadId = (SELECT TLW.LeadId 
											FROM @TC_InquiriesLeadWithId AS TLW WHERE TLW.tblId =@WhileLoopcontrol )
											WHERE Id = 	@WhileLoopcontrol
	 SET @WhileLoopcontrol=@WhileLoopcontrol+1;		
								
		END 					 						
	--Final return data
	SELECT MVC.LeadId, SUM(MVC.TotalSellerInq) AS Seller, SUM(MVC.TotalMatchStock) AS Stock FROM @TC_MatchViewSum AS MVC GROUP BY MVC.LeadId 
END