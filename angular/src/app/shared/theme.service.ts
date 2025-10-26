// تعليق: خدمة الثيم - إدارة الوضع الداكن والألوان المخصصة
import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class ThemeService {
  private storageKey = 'app.theme.dark';

  // تعليق: التحقق من الوضع الداكن
  isDark(): boolean {
    return localStorage.getItem(this.storageKey) === '1';
  }

  // تعليق: تطبيق الثيم الحالي
  applyCurrent(): void {
    const dark = this.isDark();
    document.body.classList.toggle('theme-dark', dark);
    
    // تعليق: تطبيق الألوان المخصصة من localStorage
    this.applyCustomColors();
  }

  // تعليق: تبديل الوضع الداكن/الفاتح
  toggle(): void {
    const next = !this.isDark();
    localStorage.setItem(this.storageKey, next ? '1' : '0');
    this.applyCurrent();
  }

  // تعليق: تطبيق الألوان المخصصة على CSS Variables
  private applyCustomColors(): void {
    const colorVars = ['primary', 'secondary', 'success', 'danger', 'warning', 'info'];
    
    colorVars.forEach(colorVar => {
      const savedColor = localStorage.getItem(`theme-color-${colorVar}`);
      if (savedColor) {
        document.documentElement.style.setProperty(`--color-${colorVar}`, savedColor);
        // تعليق: تطبيق على متغيرات Bootstrap أيضاً
        document.documentElement.style.setProperty(`--bs-${colorVar}`, savedColor);
      }
    });
  }

  // تعليق: حفظ لون مخصص
  setColor(colorName: string, colorValue: string): void {
    localStorage.setItem(`theme-color-${colorName}`, colorValue);
    document.documentElement.style.setProperty(`--color-${colorName}`, colorValue);
    document.documentElement.style.setProperty(`--bs-${colorName}`, colorValue);
  }

  // تعليق: الحصول على لون مخصص
  getColor(colorName: string): string | null {
    return localStorage.getItem(`theme-color-${colorName}`);
  }

  // تعليق: إعادة تعيين جميع الألوان
  resetColors(): void {
    const colorVars = ['primary', 'secondary', 'success', 'danger', 'warning', 'info'];
    colorVars.forEach(colorVar => {
      localStorage.removeItem(`theme-color-${colorVar}`);
      document.documentElement.style.removeProperty(`--color-${colorVar}`);
      document.documentElement.style.removeProperty(`--bs-${colorVar}`);
    });
  }
}


