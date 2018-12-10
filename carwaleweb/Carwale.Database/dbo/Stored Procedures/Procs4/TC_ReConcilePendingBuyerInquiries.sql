IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_ReConcilePendingBuyerInquiries]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_ReConcilePendingBuyerInquiries]
GO

	CREATE Procedure [dbo].[TC_ReConcilePendingBuyerInquiries]
as
begin
		declare @StockId numeric, @BranchId int, @dealerid numeric, @TC_TimeDuration_Id int, @TC_AddBuyerId int
		declare @customer varchar(50), @email varchar(100), @mobile varchar(12), @current_time datetime = getdate(), @sellinquiryid bigint
		declare @cnt int, @i int, @UsedCarPurchaseInquiryId bigint, @customerId bigint,@RequestDate datetime
		set @i=1

		create Table  #SellInquiries 
		(
		  id int identity(1,1),
		  UsedCarPurchaseInquiryId bigint,
		  SellInquiryId bigint,
		  CustomerId bigint,
		  RequestDate datetime
		)

		 

		--insert into @SellInquiries(UsedCarPurchaseInquiryId,SellInquiryId,CustomerId,RequestDate)
		--SELECT top 5 U.Id,S.ID,U.CustomerID,U.RequestDateTime
		--FROM   UsedCarPurchaseInquiries U 
		--       INNER JOIN SellInquiries S 
		--         ON U.SellInquiryId = S.ID 
		--WHERE  S.StatusId = 1 
		--       --AND U.RequestDateTime > Getdate() - 1 
		--       AND S.TC_StockId NOT IN(SELECT B.StockId 
		--                               FROM   TC_Inquiries I 
		--                                      INNER JOIN TC_BuyerInquiries B 
		--                                        ON B.TC_InquiriesId = I.TC_InquiriesId 
		--                               WHERE  I.SourceId = 1) 

		INSERT INTO TC_TimeDuration(Programme_Name,Starttime)
		VALUES('TC_ReConcilePendingBuyerInquiries', GETDATE())

		set @TC_TimeDuration_Id= SCOPE_IDENTITY()


		insert into #SellInquiries(UsedCarPurchaseInquiryId,SellInquiryId,CustomerId,RequestDate)
		SELECT top 50 U.Id,S.ID,U.CustomerID,U.RequestDateTime
		FROM   UsedCarPurchaseInquiries U with(nolock)
			   INNER JOIN SellInquiries S with(nolock)
				 ON U.SellInquiryId = S.ID 
		WHERE  S.StatusId = 1 
			   --AND U.RequestDateTime > Getdate() - 1 
			   AND U.Id NOT IN(SELECT B.UsedCarPurchaseInquiryId   FROM   TC_BuyerInquiries B  with(nolock) where B.UsedCarPurchaseInquiryId is not null )   
			   AND U.RequestDateTime>'2012-05-23 16:29:40.927'
		       
		  
		 
		insert into TC_MissedUsedCarInquiries(UsedCarPurchaseInquiryId ,  SellInquiryId ,  CustomerId ,  RequestDate)
		select UsedCarPurchaseInquiryId ,  SellInquiryId ,  CustomerId ,  RequestDate from #SellInquiries


		select @cnt= COUNT(id) from #SellInquiries

		print 'cnt'
		print @cnt

		print 'i'
		print @i

		while(@i<=@cnt)
		begin

				select @UsedCarPurchaseInquiryId = UsedCarPurchaseInquiryId,
					   @sellinquiryid=SellInquiryId,
					   @customerId=CustomerId,
					   @RequestDate=RequestDate
				 from #SellInquiries where id=@i
							
				select @StockId = si.TC_StockId, @BranchId = st.BranchId, @dealerid = si.DealerId 
				from SellInquiries si, TC_Stock st where si.TC_StockId = st.Id and si.ID = @sellinquiryid

				if @StockId is not null
				BEGIN	

		    
			
					-- If yes, send this inquiry back to thet dealer
						select @customer=cs.Name, @mobile=cs.Mobile, @email=cs.email from Customers cs where Id = @customerId
						
						-- Push this inquiry to trading car software by calling common procedure TC_AddBuyerInquiries 
						INSERT INTO TC_TimeDuration(Programme_Name,Starttime)
									VALUES('TC_AddBuyerInquiriesJob', GETDATE())
					                
						set @TC_AddBuyerId= SCOPE_IDENTITY()
				                		
					EXECUTE  TC_AddBuyerInquiriesJob @BranchId =@dealerid,@StockId=@StockId,@CustomerName=@customer,@Email=@email,@Mobile =@mobile,
												@Location=NULL,@Buytime=NULL,@CustomerComments=NULL,@Comments =NULL,
												@InquiryStatus =NUll,@NextFollowup =NULL,@AssignedTo =NULL,@InquirySource =1,@UserId =NULL,@UsedCarPurchaseInquiryId=@UsedCarPurchaseInquiryId,@CreatedDate=@RequestDate
					
					
					
					
						print @UsedCarPurchaseInquiryId 
						print '---'
						print @sellinquiryid
						print '---'
						print  @customerId
						print '---'
		  
				END	
		    
			set @i=@i+1
			
			update  TC_TimeDuration
			set Endtime=GETDATE()
			where  TC_TimeDuration_Id=@TC_AddBuyerId								
										
		END

		update  TC_TimeDuration
		set Endtime=GETDATE()
		where  TC_TimeDuration_Id=@TC_TimeDuration_Id

		drop table #SellInquiries

end