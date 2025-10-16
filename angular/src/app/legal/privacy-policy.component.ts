// تعليق: صفحة سياسة الخصوصية
import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-privacy-policy',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="container py-5">
      <div class="row justify-content-center">
        <div class="col-lg-10">
          <div class="card">
            <div class="card-body p-5">
              <h1 class="mb-4">
                <i class="fas fa-shield-alt me-3 text-primary"></i>
                سياسة الخصوصية
              </h1>
              
              <p class="text-muted mb-4">
                آخر تحديث: 14 أكتوبر 2025
              </p>

              <!-- تعليق: المقدمة -->
              <section class="mb-5">
                <h3>مقدمة</h3>
                <p>
                  نحن في <strong>منصة إدارة الفعاليات في سوريا</strong> نلتزم بحماية خصوصيتك وبياناتك الشخصية.
                  تشرح هذه السياسة كيفية جمع واستخدام وحماية معلوماتك.
                </p>
              </section>

              <!-- تعليق: البيانات المجمعة -->
              <section class="mb-5">
                <h3>1. البيانات التي نجمعها</h3>
                <ul>
                  <li><strong>معلومات الحساب:</strong> الاسم، البريد الإلكتروني، كلمة المرور (مشفرة)</li>
                  <li><strong>معلومات اختيارية:</strong> رقم الهاتف، المهنة، المدينة، الاهتمامات</li>
                  <li><strong>بيانات الاستخدام:</strong> الفعاليات المتابعة، الحجوزات، التفضيلات</li>
                  <li><strong>بيانات تقنية:</strong> عنوان IP، نوع المتصفح، وقت الزيارة</li>
                </ul>
              </section>

              <!-- تعليق: استخدام البيانات -->
              <section class="mb-5">
                <h3>2. كيف نستخدم بياناتك</h3>
                <ul>
                  <li>توفير خدمات المنصة (التسجيل، الحجز، التقويم)</li>
                  <li>إرسال إشعارات وتذكيرات بالفعاليات</li>
                  <li>تحسين تجربة المستخدم وتخصيص المحتوى</li>
                  <li>تحليل الاستخدام وتطوير المنصة</li>
                  <li>التواصل بشأن التحديثات والتغييرات</li>
                </ul>
              </section>

              <!-- تعليق: مشاركة البيانات -->
              <section class="mb-5">
                <h3>3. مشاركة البيانات</h3>
                <p>
                  <strong>نحن لا نبيع بياناتك الشخصية.</strong> قد نشارك بعض البيانات في الحالات التالية:
                </p>
                <ul>
                  <li><strong>مع المنظمين:</strong> اسمك ومعلومات الاتصال عند التسجيل في فعالياتهم</li>
                  <li><strong>مع السلطات:</strong> عند الطلب القانوني فقط</li>
                  <li><strong>البيانات العامة:</strong> ملفك الشخصي إذا اخترت جعله عاماً</li>
                </ul>
              </section>

              <!-- تعليق: حماية البيانات -->
              <section class="mb-5">
                <h3>4. حماية بياناتك</h3>
                <p>نستخدم تدابير أمنية متقدمة:</p>
                <ul>
                  <li>تشفير SSL/TLS لجميع البيانات المنقولة</li>
                  <li>تشفير كلمات المرور (Bcrypt)</li>
                  <li>مصادقة OAuth 2.0 + JWT</li>
                  <li>نسخ احتياطية منتظمة لقاعدة البيانات</li>
                  <li>مراقبة أمنية مستمرة</li>
                </ul>
              </section>

              <!-- تعليق: حقوقك -->
              <section class="mb-5">
                <h3>5. حقوقك</h3>
                <p>لديك الحق في:</p>
                <ul>
                  <li><strong>الوصول:</strong> عرض جميع بياناتك الشخصية</li>
                  <li><strong>التعديل:</strong> تحديث أو تصحيح معلوماتك</li>
                  <li><strong>الحذف:</strong> طلب حذف حسابك وبياناتك</li>
                  <li><strong>التصدير:</strong> الحصول على نسخة من بياناتك</li>
                  <li><strong>الاعتراض:</strong> رفض استخدامات معينة لبياناتك</li>
                </ul>
              </section>

              <!-- تعليق: ملفات الكوكيز -->
              <section class="mb-5">
                <h3>6. ملفات الكوكيز</h3>
                <p>
                  نستخدم الكوكيز لتحسين تجربتك:
                </p>
                <ul>
                  <li>كوكيز ضرورية للمصادقة والجلسة</li>
                  <li>كوكيز تحليلية لفهم استخدام المنصة</li>
                  <li>كوكيز التفضيلات (اللغة، الثيم)</li>
                </ul>
                <p>يمكنك إدارة الكوكيز من إعدادات المتصفح.</p>
              </section>

              <!-- تعليق: الاتصال بنا -->
              <section class="mb-5">
                <h3>7. الاتصال بنا</h3>
                <p>
                  لأي استفسارات حول خصوصيتك:
                </p>
                <ul>
                  <li><strong>البريد الإلكتروني:</strong> privacy&#64;eventmanagement.sy</li>
                  <li><strong>الهاتف:</strong> +963 11 xxx xxxx</li>
                </ul>
              </section>

              <!-- تعليق: التحديثات -->
              <section>
                <h3>8. تحديثات السياسة</h3>
                <p>
                  قد نحدث هذه السياسة من وقت لآخر. سنخطرك بأي تغييرات جوهرية عبر البريد الإلكتروني.
                </p>
              </section>

              <!-- زر العودة -->
              <div class="mt-5 text-center">
                <a href="/" class="btn btn-primary btn-lg">
                  <i class="fas fa-home me-2"></i>
                  العودة للرئيسية
                </a>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  `
})
export class PrivacyPolicyComponent {}

