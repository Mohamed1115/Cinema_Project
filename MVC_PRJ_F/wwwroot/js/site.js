// ============================================
// Cinema Project - Enhanced Interactive JavaScript
// Modern interactive features and utilities
// ============================================

(function() {
    'use strict';

    // ============================================
    // Utility Functions
    // ============================================
    
    const CinemaUtils = {
        // Debounce function for performance
        debounce: function(func, wait) {
            let timeout;
            return function executedFunction(...args) {
                const later = () => {
                    clearTimeout(timeout);
                    func(...args);
                };
                clearTimeout(timeout);
                timeout = setTimeout(later, wait);
            };
        },

        // Smooth scroll to element
        smoothScroll: function(target) {
            const element = document.querySelector(target);
            if (element) {
                element.scrollIntoView({
                    behavior: 'smooth',
                    block: 'start'
                });
            }
        },

        // Show loading overlay
        showLoading: function() {
            let overlay = document.getElementById('loadingOverlay');
            if (!overlay) {
                overlay = document.createElement('div');
                overlay.id = 'loadingOverlay';
                overlay.className = 'loading-overlay';
                overlay.innerHTML = '<div class="spinner-cinema"></div>';
                document.body.appendChild(overlay);
            }
            setTimeout(() => overlay.classList.add('active'), 10);
        },

        // Hide loading overlay
        hideLoading: function() {
            const overlay = document.getElementById('loadingOverlay');
            if (overlay) {
                overlay.classList.remove('active');
            }
        },

        // Show toast notification
        showToast: function(message, type = 'info', duration = 3000) {
            const toast = document.createElement('div');
            toast.className = `alert alert-${type} alert-enhanced position-fixed top-0 end-0 m-3`;
            toast.style.zIndex = '9999';
            toast.innerHTML = `
                ${message}
                <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
            `;
            document.body.appendChild(toast);

            setTimeout(() => {
                toast.classList.add('fade');
                setTimeout(() => toast.remove(), 300);
            }, duration);
        }
    };

    // ============================================
    // Auto-hide Alerts
    // ============================================
    
    function initAutoHideAlerts() {
        const alerts = document.querySelectorAll('.alert:not(.alert-permanent)');
        alerts.forEach(alert => {
            setTimeout(() => {
                const bsAlert = new bootstrap.Alert(alert);
                bsAlert.close();
            }, 5000);
        });
    }

    // ============================================
    // Smooth Scroll for Anchor Links
    // ============================================
    
    function initSmoothScroll() {
        document.querySelectorAll('a[href^="#"]').forEach(anchor => {
            anchor.addEventListener('click', function(e) {
                const href = this.getAttribute('href');
                if (href !== '#' && href !== '#!') {
                    e.preventDefault();
                    CinemaUtils.smoothScroll(href);
                }
            });
        });
    }

    // ============================================
    // Image Lazy Loading
    // ============================================
    
    function initLazyLoading() {
        const images = document.querySelectorAll('img[data-src]');
        
        const imageObserver = new IntersectionObserver((entries, observer) => {
            entries.forEach(entry => {
                if (entry.isIntersecting) {
                    const img = entry.target;
                    img.src = img.dataset.src;
                    img.classList.add('fade-in');
                    img.removeAttribute('data-src');
                    observer.unobserve(img);
                }
            });
        });

        images.forEach(img => imageObserver.observe(img));
    }

    // ============================================
    // Form Validation Enhancement
    // ============================================
    
    function initFormValidation() {
        const forms = document.querySelectorAll('.needs-validation');
        
        forms.forEach(form => {
            form.addEventListener('submit', function(event) {
                if (!form.checkValidity()) {
                    event.preventDefault();
                    event.stopPropagation();
                }
                form.classList.add('was-validated');
            }, false);

            // Real-time validation
            const inputs = form.querySelectorAll('input, select, textarea');
            inputs.forEach(input => {
                input.addEventListener('blur', function() {
                    if (this.checkValidity()) {
                        this.classList.remove('is-invalid');
                        this.classList.add('is-valid');
                    } else {
                        this.classList.remove('is-valid');
                        this.classList.add('is-invalid');
                    }
                });
            });
        });
    }

    // ============================================
    // Password Visibility Toggle
    // ============================================
    
    function initPasswordToggle() {
        const toggleButtons = document.querySelectorAll('[data-password-toggle]');
        
        toggleButtons.forEach(button => {
            button.addEventListener('click', function() {
                const targetId = this.getAttribute('data-password-toggle');
                const input = document.getElementById(targetId);
                const icon = this.querySelector('i');
                
                if (input.type === 'password') {
                    input.type = 'text';
                    icon.classList.remove('fa-eye');
                    icon.classList.add('fa-eye-slash');
                } else {
                    input.type = 'password';
                    icon.classList.remove('fa-eye-slash');
                    icon.classList.add('fa-eye');
                }
            });
        });
    }

    // ============================================
    // Image Preview for File Upload
    // ============================================
    
    function initImagePreview() {
        const fileInputs = document.querySelectorAll('input[type="file"][accept*="image"]');
        
        fileInputs.forEach(input => {
            input.addEventListener('change', function(e) {
                const file = e.target.files[0];
                if (file) {
                    const reader = new FileReader();
                    reader.onload = function(e) {
                        let preview = input.parentElement.querySelector('.image-preview');
                        if (!preview) {
                            preview = document.createElement('div');
                            preview.className = 'image-preview mt-3';
                            input.parentElement.appendChild(preview);
                        }
                        preview.innerHTML = `
                            <img src="${e.target.result}" class="img-fluid rounded-cinema" 
                                 style="max-height: 200px;" alt="Preview">
                        `;
                    };
                    reader.readAsDataURL(file);
                }
            });
        });
    }

    // ============================================
    // Search Filter
    // ============================================
    
    function initSearchFilter() {
        const searchInputs = document.querySelectorAll('[data-search-target]');
        
        searchInputs.forEach(input => {
            const targetSelector = input.getAttribute('data-search-target');
            const items = document.querySelectorAll(targetSelector);
            
            input.addEventListener('input', CinemaUtils.debounce(function(e) {
                const searchTerm = e.target.value.toLowerCase();
                
                items.forEach(item => {
                    const text = item.textContent.toLowerCase();
                    if (text.includes(searchTerm)) {
                        item.style.display = '';
                        item.classList.add('fade-in');
                    } else {
                        item.style.display = 'none';
                    }
                });
            }, 300));
        });
    }

    // ============================================
    // Animated Counter
    // ============================================
    
    function initAnimatedCounters() {
        const counters = document.querySelectorAll('[data-counter]');
        
        const counterObserver = new IntersectionObserver((entries) => {
            entries.forEach(entry => {
                if (entry.isIntersecting) {
                    const counter = entry.target;
                    const target = parseInt(counter.getAttribute('data-counter'));
                    const duration = 2000;
                    const increment = target / (duration / 16);
                    let current = 0;
                    
                    const updateCounter = () => {
                        current += increment;
                        if (current < target) {
                            counter.textContent = Math.floor(current);
                            requestAnimationFrame(updateCounter);
                        } else {
                            counter.textContent = target;
                        }
                    };
                    
                    updateCounter();
                    counterObserver.unobserve(counter);
                }
            });
        }, { threshold: 0.5 });
        
        counters.forEach(counter => counterObserver.observe(counter));
    }

    // ============================================
    // Lightbox for Images
    // ============================================
    
    function initLightbox() {
        const lightboxImages = document.querySelectorAll('[data-lightbox]');
        
        if (lightboxImages.length > 0) {
            // Create lightbox modal
            const lightboxModal = document.createElement('div');
            lightboxModal.id = 'lightboxModal';
            lightboxModal.className = 'modal fade';
            lightboxModal.innerHTML = `
                <div class="modal-dialog modal-dialog-centered modal-xl">
                    <div class="modal-content bg-transparent border-0">
                        <div class="modal-body p-0 text-center">
                            <button type="button" class="btn-close btn-close-white position-absolute top-0 end-0 m-3" 
                                    data-bs-dismiss="modal"></button>
                            <img src="" class="img-fluid rounded-cinema" alt="Lightbox Image">
                        </div>
                    </div>
                </div>
            `;
            document.body.appendChild(lightboxModal);
            
            const modal = new bootstrap.Modal(lightboxModal);
            const modalImg = lightboxModal.querySelector('img');
            
            lightboxImages.forEach(img => {
                img.style.cursor = 'pointer';
                img.addEventListener('click', function() {
                    modalImg.src = this.src;
                    modalImg.alt = this.alt;
                    modal.show();
                });
            });
        }
    }

    // ============================================
    // Confirmation Dialog
    // ============================================
    
    function initConfirmationDialogs() {
        const confirmButtons = document.querySelectorAll('[data-confirm]');
        
        confirmButtons.forEach(button => {
            button.addEventListener('click', function(e) {
                const message = this.getAttribute('data-confirm');
                if (!confirm(message)) {
                    e.preventDefault();
                    return false;
                }
            });
        });
    }

    // ============================================
    // Scroll to Top Button
    // ============================================
    
    function initScrollToTop() {
        const scrollBtn = document.createElement('button');
        scrollBtn.id = 'scrollToTop';
        scrollBtn.className = 'btn btn-primary btn-cinema position-fixed bottom-0 end-0 m-4';
        scrollBtn.style.cssText = 'display: none; z-index: 1000; border-radius: 50%; width: 50px; height: 50px;';
        scrollBtn.innerHTML = '<i class="fas fa-arrow-up"></i>';
        document.body.appendChild(scrollBtn);
        
        window.addEventListener('scroll', () => {
            if (window.pageYOffset > 300) {
                scrollBtn.style.display = 'block';
                scrollBtn.classList.add('fade-in');
            } else {
                scrollBtn.style.display = 'none';
            }
        });
        
        scrollBtn.addEventListener('click', () => {
            window.scrollTo({
                top: 0,
                behavior: 'smooth'
            });
        });
    }

    // ============================================
    // Active Navigation Highlighting
    // ============================================
    
    function initActiveNavigation() {
        const currentPath = window.location.pathname;
        const navLinks = document.querySelectorAll('.nav-link');
        
        navLinks.forEach(link => {
            const href = link.getAttribute('href');
            if (href && currentPath.includes(href) && href !== '/') {
                link.classList.add('active');
            }
        });
    }

    // ============================================
    // Card Hover Effects
    // ============================================
    
    function initCardEffects() {
        const cards = document.querySelectorAll('.card-movie, .card-enhanced');
        
        cards.forEach(card => {
            card.addEventListener('mouseenter', function() {
                this.style.zIndex = '10';
            });
            
            card.addEventListener('mouseleave', function() {
                this.style.zIndex = '';
            });
        });
    }

    // ============================================
    // Initialize All Features
    // ============================================
    
    function init() {
        // Wait for DOM to be ready
        if (document.readyState === 'loading') {
            document.addEventListener('DOMContentLoaded', init);
            return;
        }

        console.log('🎬 Cinema Enhanced JS Loaded');

        // Initialize all features
        initAutoHideAlerts();
        initSmoothScroll();
        initLazyLoading();
        initFormValidation();
        initPasswordToggle();
        initImagePreview();
        initSearchFilter();
        initAnimatedCounters();
        initLightbox();
        initConfirmationDialogs();
        initScrollToTop();
        initActiveNavigation();
        initCardEffects();

        // Hide loading overlay if present
        setTimeout(() => CinemaUtils.hideLoading(), 500);
    }

    // Start initialization
    init();

    // Expose utilities globally
    window.CinemaUtils = CinemaUtils;

})();

// ============================================
// Additional Helper Functions
// ============================================

// Format currency
function formatCurrency(amount, currency = 'USD') {
    return new Intl.NumberFormat('en-US', {
        style: 'currency',
        currency: currency
    }).format(amount);
}

// Format date
function formatDate(date, locale = 'en-US') {
    return new Intl.DateTimeFormat(locale, {
        year: 'numeric',
        month: 'long',
        day: 'numeric'
    }).format(new Date(date));
}

// Truncate text
function truncateText(text, maxLength) {
    if (text.length <= maxLength) return text;
    return text.substr(0, maxLength) + '...';
}
